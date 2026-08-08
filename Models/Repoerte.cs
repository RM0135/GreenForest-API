using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenForest.Models;

public class Reporte
{
    public int Id { get; set; }

    public DateTime FechaGeneracion { get; set; }

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    public int CantidadArboles { get; set; }

    public string Observaciones { get; set; } = string.Empty;

    // Llave foránea
    public int ProyectoId { get; set; }

    [ForeignKey("ProyectoId")]
    public Proyecto? Proyecto { get; set; }
}