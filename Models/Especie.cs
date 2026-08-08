using System.ComponentModel.DataAnnotations;

namespace GreenForest.Models;

public class Especie
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string NombreComun { get; set; } = string.Empty;

    [MaxLength(150)]
    public string NombreCientifico { get; set; } = string.Empty;

    // Distancia recomendada entre árboles en metros
    public decimal DistanciaSiembra { get; set; }

    [MaxLength(100)]
    public string ClimaIdeal { get; set; } = string.Empty;

    // Datos utilizados para determinar compatibilidad climática
    public decimal TemperaturaMinima { get; set; }

    public decimal TemperaturaMaxima { get; set; }

    // Precipitación anual en milímetros
    public decimal PrecipitacionMinima { get; set; }

    public decimal PrecipitacionMaxima { get; set; }

    // Humedad relativa en porcentaje
    public decimal HumedadMinima { get; set; }

    public decimal HumedadMaxima { get; set; }

    [MaxLength(100)]
    public string TipoSuelo { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ToleranciaSequía { get; set; } = string.Empty;

    // Relaciones
    public ICollection<Arbol> Arboles { get; set; } = new List<Arbol>();
}