using System.ComponentModel.DataAnnotations;

namespace GreenForest.Models;

public class Organizacion
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Tipo { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    // Relaciones
    public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
