using System.Text;
using System.Text.Json;

namespace GreenForest.AI;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    private const string ContextoBase =
        "Eres un experto forestal especializado en proyectos de reforestación. " +
        "Responde de forma breve, concreta y directa, basándote en criterios técnicos " +
        "reales (silvicultura, agronomía y ecología). No des rodeos ni explicaciones " +
        "largas: ve directo a la recomendación práctica. Máximo 3-4 líneas por respuesta, " +
        "salvo que se pidan explícitamente varias opciones en lista.";

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    // ---------- Métodos públicos específicos de reforestación ----------

    public Task<string> RecomendarEspecie(string clima, string tipoSuelo, string region)
    {
        var prompt = $"""
            {ContextoBase}

            Recomienda 3 a 5 especies de árboles nativos o adaptados para reforestación
            considerando:
            - Clima: {clima}
            - Tipo de suelo: {tipoSuelo}
            - Región: {region}

            Para cada especie indica: nombre común, nombre científico, y por qué es
            adecuada para estas condiciones.
            """;

        return PreguntarGemini(prompt);
    }

    public Task<string> DistanciaOptima(string especie, string proposito)
    {
        var prompt = $"""
            {ContextoBase}

            Indica la distancia óptima de plantado (en metros) entre individuos de la
            especie "{especie}" cuando el propósito de la plantación es: {proposito}.

            Explica brevemente por qué esa distancia (competencia por luz, raíces,
            crecimiento de copa) y si cambia según densidad de plantación (restauración
            vs. producción vs. cortina rompevientos).
            """;

        return PreguntarGemini(prompt);
    }

    public Task<string> ArbolOptimoParaClima(string clima, string temperaturaPromedio, string precipitacionAnual)
    {
        var prompt = $"""
            {ContextoBase}

            Según estas condiciones climáticas:
            - Clima general: {clima}
            - Temperatura promedio: {temperaturaPromedio}
            - Precipitación anual: {precipitacionAnual}

            Indica cuál sería el árbol o los 3 mejores árboles para plantar, explicando
            su tolerancia a estas condiciones (sequía, heladas, humedad).
            """;

        return PreguntarGemini(prompt);
    }

    // Por si en algún punto necesitas una pregunta libre, pero igual con el contexto de reforestación
    public Task<string> PreguntarLibre(string pregunta)
    {
        var prompt = $"{ContextoBase}\n\nPregunta del usuario: {pregunta}";
        return PreguntarGemini(prompt);
    }

    // ---------- Único método que habla con la API de Gemini ----------

    private async Task<string> PreguntarGemini(string pregunta)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        var modelo = _configuration["Gemini:Model"];

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apiKey}";

        Console.WriteLine("URL:");
        Console.WriteLine(url);

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
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var resultado = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Status: " + response.StatusCode);
        Console.WriteLine("Respuesta:");
        Console.WriteLine(resultado);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(resultado);
        }

        using JsonDocument doc = JsonDocument.Parse(resultado);

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "";
    }
}