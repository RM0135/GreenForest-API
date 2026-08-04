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

    public decimal DistanciaSiembra { get; set; }

    [MaxLength(100)]
    public string ClimaIdeal { get; set; } = string.Empty;

    // Relaciones
    public ICollection<Arbol> Arboles { get; set; } = new List<Arbol>();
}
