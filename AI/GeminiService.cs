using System.Text;
using System.Text.Json;
using GreenForest.Data;
using GreenForest.DTO;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.AI
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly GreenForestContext _context;

        public GeminiService(
            HttpClient httpClient,
            IConfiguration configuration,
            GreenForestContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        // ============================================================
        // CHAT GENERAL CON GEMINI
        // ============================================================

       public async Task<string> PreguntarGemini(string pregunta)
                {
                    var apiKey = _configuration["Gemini:ApiKey"];
                    var modelo = _configuration["Gemini:Model"];

                    if (string.IsNullOrWhiteSpace(apiKey))
                    {
                        throw new Exception(
                            "La API Key de Gemini no está configurada."
                        );
                    }

                    if (string.IsNullOrWhiteSpace(modelo))
                    {
                        modelo = "gemini-flash-latest";
                    }

                    var url =
                        $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apiKey}";

                    var body = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new
                                    {
                                        text = pregunta
                                    }
                                }
                            }
                        }
                    };

                    var json = JsonSerializer.Serialize(body);

                    // ============================================================
                    // REINTENTOS AUTOMÁTICOS
                    // ============================================================

                    const int maxIntentos = 3;

                    for (int intento = 1; intento <= maxIntentos; intento++)
                    {
                        using var content = new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json"
                        );

                        var response = await _httpClient.PostAsync(
                            url,
                            content
                        );

                        var resultado =
                            await response.Content.ReadAsStringAsync();

                        // ========================================================
                        // GEMINI ESTÁ DISPONIBLE
                        // ========================================================

                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                using JsonDocument doc =
                                    JsonDocument.Parse(resultado);

                                return doc.RootElement
                                    .GetProperty("candidates")[0]
                                    .GetProperty("content")
                                    .GetProperty("parts")[0]
                                    .GetProperty("text")
                                    .GetString() ?? "";
                            }
                            catch
                            {
                                throw new Exception(
                                    "Gemini respondió, pero no fue posible interpretar la respuesta."
                                );
                            }
                        }

                        // ========================================================
                        // ERROR 503 - SERVICIO TEMPORALMENTE NO DISPONIBLE
                        // ========================================================

                        if (
                            response.StatusCode ==
                            System.Net.HttpStatusCode.ServiceUnavailable
                        )
                        {
                            if (intento < maxIntentos)
                            {
                                int segundosEspera = intento * 2;

                                await Task.Delay(
                                    TimeSpan.FromSeconds(segundosEspera)
                                );

                                continue;
                            }

                            throw new Exception(
                                "Gemini está temporalmente saturado. " +
                                "Se realizaron 3 intentos y el servicio no estuvo disponible. " +
                                "Intenta nuevamente en unos minutos."
                            );
                        }

                        // ========================================================
                        // ERROR 429 - DEMASIADAS SOLICITUDES
                        // ========================================================

                        if (
                            response.StatusCode ==
                            System.Net.HttpStatusCode.TooManyRequests
                        )
                        {
                            if (intento < maxIntentos)
                            {
                                int segundosEspera = intento * 3;

                                await Task.Delay(
                                    TimeSpan.FromSeconds(segundosEspera)
                                );

                                continue;
                            }

                            throw new Exception(
                                "Se alcanzó temporalmente el límite de solicitudes de Gemini. " +
                                "Intenta nuevamente en unos minutos."
                            );
                        }

                        // ========================================================
                        // OTROS ERRORES
                        // ========================================================

                        throw new Exception(
                            $"Error al comunicarse con Gemini. " +
                            $"Código: {response.StatusCode}. " +
                            $"Respuesta: {resultado}"
                        );
                    }

                    throw new Exception(
                        "No fue posible obtener una respuesta de Gemini."
                    );
                }
        // ============================================================
        // RECOMENDACIÓN DE REFORESTACIÓN
        // ============================================================

                    public async Task<RecomendacionReforestacionDTO>
                RecomendarReforestacion(
                    int proyectoId,
                    decimal temperatura,
                    decimal precipitacion,
                    decimal humedad,
                    string tipoSuelo)
            {
                // ============================================================
                // 1. BUSCAR EL PROYECTO
                // ============================================================

                var proyecto = await _context.Proyectos
                    .Include(p => p.Arboles)
                    .FirstOrDefaultAsync(p => p.Id == proyectoId);

                if (proyecto == null)
                {
                    throw new Exception(
                        "El proyecto de reforestación no existe."
                    );
                }

                // ============================================================
                // 2. OBTENER LAS ESPECIES
                // ============================================================

                var especies = await _context.Especies
                    .AsNoTracking()
                    .ToListAsync();

                if (especies.Count == 0)
                {
                    throw new Exception(
                        "No existen especies registradas en la base de datos."
                    );
                }

                // ============================================================
                // 3. CALCULAR COMPATIBILIDAD
                // ============================================================

                var especiesAnalizadas = especies
                    .Select(especie =>
                    {
                        decimal puntuacion = 0;

                        // ============================================================
                        // TEMPERATURA = 35%
                        // ============================================================

                        decimal puntuacionTemperatura = 0;

                        decimal temperaturaMinima =
                            Convert.ToDecimal(especie.TemperaturaMinima);

                        decimal temperaturaMaxima =
                            Convert.ToDecimal(especie.TemperaturaMaxima);

                        if (
                            temperatura >= temperaturaMinima &&
                            temperatura <= temperaturaMaxima
                        )
                        {
                            // La temperatura está dentro del rango.
                            // Mientras más cerca del centro del rango,
                            // mayor será la puntuación.

                            decimal temperaturaIdeal =
                                (temperaturaMinima + temperaturaMaxima) / 2;

                            decimal rangoTemperatura =
                                (temperaturaMaxima - temperaturaMinima) / 2;

                            if (rangoTemperatura > 0)
                            {
                                decimal diferencia =
                                    Math.Abs(
                                        temperatura - temperaturaIdeal
                                    );

                               puntuacionTemperatura =
                                    35 *
                                    Math.Max(
                                        0,
                                        1 - (diferencia / rangoTemperatura)
                                    );
                            }
                            else
                            {
                                puntuacionTemperatura = 40;
                            }
                        }

                        // ============================================================
                        // PRECIPITACIÓN = 35%
                        // ============================================================

                        decimal puntuacionPrecipitacion = 0;

                        decimal precipitacionMinima =
                            Convert.ToDecimal(
                                especie.PrecipitacionMinima
                            );

                        decimal precipitacionMaxima =
                            Convert.ToDecimal(
                                especie.PrecipitacionMaxima
                            );

                        if (
                            precipitacion >= precipitacionMinima &&
                            precipitacion <= precipitacionMaxima
                        )
                        {
                            decimal precipitacionIdeal =
                                (
                                    precipitacionMinima +
                                    precipitacionMaxima
                                ) / 2;

                            decimal rangoPrecipitacion =
                                (
                                    precipitacionMaxima -
                                    precipitacionMinima
                                ) / 2;

                            if (rangoPrecipitacion > 0)
                            {
                                decimal diferencia =
                                    Math.Abs(
                                        precipitacion -
                                        precipitacionIdeal
                                    );

                                puntuacionPrecipitacion =
                                    35 *
                                    Math.Max(
                                        0,
                                        1 - (
                                            diferencia /
                                            rangoPrecipitacion
                                        )
                                    );
                            }
                            else
                            {
                                puntuacionPrecipitacion = 40;
                            }
                        }

                        // ============================================================
                        // HUMEDAD = 15%
                        // ============================================================

                            decimal puntuacionHumedad = 0;

                            decimal humedadMinima =
                                Convert.ToDecimal(especie.HumedadMinima);

                            decimal humedadMaxima =
                                Convert.ToDecimal(especie.HumedadMaxima);

                            if (
                                humedad >= humedadMinima &&
                                humedad <= humedadMaxima
                            )
                            {
                                decimal humedadIdeal =
                                    (humedadMinima + humedadMaxima) / 2;

                                decimal rangoHumedad =
                                    (humedadMaxima - humedadMinima) / 2;

                                if (rangoHumedad > 0)
                                {
                                    decimal diferencia =
                                        Math.Abs(humedad - humedadIdeal);

                                    puntuacionHumedad =
                                        15 *
                                        Math.Max(
                                            0,
                                            1 - (diferencia / rangoHumedad)
                                        );
                                }
                                else
                                {
                                    puntuacionHumedad = 15;
                                }
                            }

                        // ============================================================
                        // TIPO DE SUELO = 20%
                        // ============================================================

                        decimal puntuacionSuelo = 0;

                       if 
                        (
                            !string.IsNullOrWhiteSpace(tipoSuelo) &&
                            !string.IsNullOrWhiteSpace(especie.TipoSuelo) &&
                            especie.TipoSuelo.Contains(
                                tipoSuelo,
                                StringComparison.OrdinalIgnoreCase
                            )       
                        )
                            {
                                puntuacionSuelo = 15;
                            }
                        
                            // ============================================================
                            // PUNTUACIÓN FINAL
                            // ============================================================

                            puntuacion =
                            puntuacionTemperatura +
                            puntuacionPrecipitacion +
                            puntuacionHumedad +
                            puntuacionSuelo;
                       return new
                        {
                            Especie = especie,
                            Compatibilidad = Math.Round(puntuacion, 2)
                        };
                    })
                    .OrderByDescending(x => x.Compatibilidad)
                    .ToList();

                // ============================================================
                // 4. ESPECIE CON MAYOR COMPATIBILIDAD
                // ============================================================

                var mejorEspecie = especiesAnalizadas.First();

                var especieRecomendada = mejorEspecie.Especie;

                var compatibilidad =
                    mejorEspecie.Compatibilidad;

                // ============================================================
                // 5. CALCULAR CANTIDAD DE ÁRBOLES
                // ============================================================

                decimal hectareas = proyecto.MetaHectareas;

                decimal distancia =
                    Convert.ToDecimal(
                        especieRecomendada.DistanciaSiembra
                    );

                int arbolesEstimados = 0;

                if (hectareas > 0 && distancia > 0)
                {
                    // 1 hectárea = 10.000 m²
                    decimal areaMetrosCuadrados =
                        hectareas * 10000;

                    // Se considera una distribución
                    // aproximada en cuadrícula.
                    decimal areaPorArbol =
                        distancia * distancia;

                    arbolesEstimados =
                        (int)Math.Floor(
                            areaMetrosCuadrados /
                            areaPorArbol
                        );
                }

                            // ============================================================
                            // 6. ÁRBOLES ACTUALMENTE REGISTRADOS
                            // ============================================================

                            // ============================================================
                            // 6. DATOS DE AVANCE DEL PROYECTO
                            // ============================================================

                            int arbolesActuales =
                            proyecto.Arboles?.Count ?? 0;

                            int metaArboles =
                             proyecto.MetaArboles;

                            // ============================================================
                            // ÁRBOLES FALTANTES
                            // ============================================================

                                    int arbolesFaltantes =
                                        Math.Max(
                                            metaArboles - arbolesActuales,
                                0
                            );

                            // ============================================================
                            // PORCENTAJE DE CUMPLIMIENTO
                            // ============================================================

                            decimal porcentajeCumplimiento = 0;

                            if (metaArboles > 0)
                            {
                                porcentajeCumplimiento =
                                    Math.Round(
                                        (
                                            (decimal)arbolesActuales /
                                            metaArboles
                                        ) * 100,
                                        2
                                    );
                            }

                            // ============================================================
                            // META ALCANZADA
                            // ============================================================

                bool metaAlcanzada =
                arbolesActuales >= metaArboles;
                // ============================================================
                // 7. CREAR INFORMACIÓN DE ESPECIES
                // ============================================================

                var informacionEspecies =
                    new StringBuilder();

                foreach (var item in especiesAnalizadas)
                {
                    var especie = item.Especie;

                    informacionEspecies.AppendLine(
                        $"Nombre común: {especie.NombreComun}"
                    );

                    informacionEspecies.AppendLine(
                        $"Nombre científico: {especie.NombreCientifico}"
                    );

                    informacionEspecies.AppendLine(
                        $"Distancia de siembra: " +
                        $"{especie.DistanciaSiembra} metros"
                    );

                    informacionEspecies.AppendLine(
                        $"Clima ideal: {especie.ClimaIdeal}"
                    );

                    informacionEspecies.AppendLine(
                        $"Temperatura mínima: " +
                        $"{especie.TemperaturaMinima} °C"
                    );

                    informacionEspecies.AppendLine(
                        $"Temperatura máxima: " +
                        $"{especie.TemperaturaMaxima} °C"
                    );

                    informacionEspecies.AppendLine(
                        $"Precipitación mínima: " +
                        $"{especie.PrecipitacionMinima} mm"
                    );

                    informacionEspecies.AppendLine(
                        $"Precipitación máxima: " +
                        $"{especie.PrecipitacionMaxima} mm"
                    );

                    informacionEspecies.AppendLine(
                    $"Humedad mínima: " +
                    $"{especie.HumedadMinima} %"
                    );

                    informacionEspecies.AppendLine(
                    $"Humedad máxima: " +
                    $"{especie.HumedadMaxima} %"
                    );

                    informacionEspecies.AppendLine(
                        $"Tipo de suelo: {especie.TipoSuelo}"
                    );

                    informacionEspecies.AppendLine(
                        $"Compatibilidad calculada por GreenForest: " +
                        $"{item.Compatibilidad}%"
                    );

                    informacionEspecies.AppendLine(
                        "------------------------------------"
                    );
                }

                // ============================================================
                // 8. ESTRUCTURA JSON
                // ============================================================

            var estructuraJson = @"
                {
                    ""especieRecomendada"": ""nombre común"",
                    ""nombreCientifico"": ""nombre científico"",
                    ""distanciaSiembra"": 0,
                    ""climaIdeal"": ""clima"",
                    ""compatibilidad"": 0,
                    ""arbolesEstimados"": 0,
                    ""metaArboles"": 0,
                    ""arbolesActuales"": 0,
                    ""arbolesFaltantes"": 0,
                    ""porcentajeCumplimiento"": 0,
                    ""metaAlcanzada"": false,
                    ""justificacion"": ""explicación"",
                    ""recomendaciones"": [
                        ""recomendación 1"",
                        ""recomendación 2"",
                        ""recomendación 3""
                    ]
                }";

                // ============================================================
                // 9. CREAR PROMPT PARA GEMINI
                // ============================================================

                var prompt = $@"
            Eres el asistente especializado en reforestación
            de GreenForest.

            Tu función es analizar proyectos de reforestación
            y explicar cuál es la especie más adecuada.

            IMPORTANTE:

            Debes utilizar únicamente los datos proporcionados
            por GreenForest.

            No inventes especies.

            No inventes distancias.

            No inventes datos climáticos.

            No cambies la compatibilidad calculada por GreenForest.

            No cambies la cantidad de árboles estimada calculada por GreenForest.

            No cambies la meta de árboles del proyecto.

            No cambies la cantidad de árboles actualmente registrados.

            No cambies la cantidad de árboles faltantes.

            No cambies el porcentaje de cumplimiento.

            No cambies el valor de metaAlcanzada.

            Estos valores son calculados por el sistema y Gemini
            solamente debe explicarlos.

            No calcules nuevamente la cantidad de árboles.

            ----------------------------------------------------
            DATOS DEL PROYECTO
            ----------------------------------------------------

            Proyecto:
            {proyecto.Nombre}

            Descripción:
            {proyecto.Descripcion}

            Meta de árboles:
            {proyecto.MetaArboles}

            Meta de hectáreas:
            {proyecto.MetaHectareas}

            Árboles actualmente registrados:
            {arbolesActuales}

            Meta de árboles del proyecto:
            {metaArboles}

            Árboles faltantes para alcanzar la meta:
            {arbolesFaltantes}

            Porcentaje de cumplimiento:
            {porcentajeCumplimiento}%

            ¿La meta ya fue alcanzada?:
            {metaAlcanzada}

            ----------------------------------------------------
            CONDICIONES AMBIENTALES
            ----------------------------------------------------

            Temperatura:
            {temperatura} °C

            Precipitación:
            {precipitacion} mm

            Humedad:
            {humedad} %

            Tipo de suelo:
            {tipoSuelo}

            ----------------------------------------------------
            RESULTADO CALCULADO POR GREENFOREST
            ----------------------------------------------------

            Especie con mayor compatibilidad:
            {especieRecomendada.NombreComun}

            Nombre científico:
            {especieRecomendada.NombreCientifico}

            Compatibilidad:
            {compatibilidad} %

            Distancia de siembra:
            {distancia} metros

            Área disponible:
            {hectareas} hectáreas

            Capacidad aproximada del terreno:
            {arbolesEstimados} árboles

            Esta capacidad se calcula usando el área del proyecto
            y la distancia de plantación recomendada.

            No debes modificar este valor.
            ----------------------------------------------------
            ESPECIES DISPONIBLES
            ----------------------------------------------------

            {informacionEspecies}

            ----------------------------------------------------
            TAREA
            ----------------------------------------------------

            1. Recomienda la especie con mayor compatibilidad.

            2. Utiliza exactamente la distancia de siembra
            registrada en la base de datos.

            3. Utiliza exactamente la compatibilidad calculada
            por GreenForest.

            4. Utiliza exactamente la cantidad de árboles
            calculada por GreenForest.

            5. Analiza las condiciones climáticas proporcionadas.

            6.Explica por qué la especie es adecuada considerando temperatura,
            precipitación, humedad y tipo de suelo.

            7. Proporciona recomendaciones para la plantación.

            8. No inventes información.

            9. Explica el avance del proyecto respecto a su meta.

            10. Si la meta no ha sido alcanzada, indica cuántos árboles
            faltan.

            11. Si la meta ya fue alcanzada, indícalo claramente.

            12. Compara la capacidad estimada del terreno con la meta
            del proyecto.

            13. Genera recomendaciones prácticas de plantación y
            seguimiento.

            ----------------------------------------------------
            FORMATO DE RESPUESTA
            ----------------------------------------------------

            Devuelve únicamente un objeto JSON válido.

            La estructura debe ser:

            {estructuraJson}

            No utilices Markdown.

            No utilices bloques de código.

            No escribas texto antes del JSON.

            No escribas texto después del JSON.
            ";

                // ============================================================
                // 10. ENVIAR A GEMINI
                // ============================================================

                var respuestaGemini =
                    await PreguntarGemini(prompt);

                // ============================================================
                // 11. LIMPIAR RESPUESTA
                // ============================================================

                respuestaGemini = respuestaGemini
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                // ============================================================
                // 12. CONVERTIR RESPUESTA A DTO
                // ============================================================

                try
                {
                    var resultado =
                        JsonSerializer.Deserialize
                        <RecomendacionReforestacionDTO>(
                            respuestaGemini,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        );

                    if (resultado == null)
                    {
                        throw new Exception(
                            "Gemini devolvió una respuesta vacía."
                        );
                    }

                    return resultado;
                }
                catch (JsonException)
                {
                    throw new Exception(
                        "Gemini no devolvió un JSON válido.\n\n" +
                        "Respuesta recibida:\n" +
                        respuestaGemini
                    );
                 }
        } 
    }
}
