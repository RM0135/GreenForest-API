using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenForest.Models;

public class Proyecto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = "Activo";

    public int MetaArboles { get; set; }

    public decimal MetaHectareas { get; set; }

    // Llave foránea: usuario responsable del proyecto
    public int UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    // Llave foránea: organización asociada (opcional)
    public int? OrganizacionId { get; set; }

    [ForeignKey("OrganizacionId")]
    public Organizacion? Organizacion { get; set; }

    // Relaciones
    public ICollection<Arbol> Arboles { get; set; } = new List<Arbol>();

    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}