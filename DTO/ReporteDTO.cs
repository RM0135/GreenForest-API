namespace GreenForest.DTOs;

public class ReporteDTO
{
    public DateTime FechaGeneracion { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public int CantidadArboles { get; set; }

    public string Observaciones { get; set; } = string.Empty;

    public int ProyectoId { get; set; }
}