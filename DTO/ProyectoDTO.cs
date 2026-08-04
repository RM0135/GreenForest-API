namespace GreenForest.DTOs;

public class ProyectoDTO
{
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public int MetaArboles { get; set; }

    public decimal MetaHectareas { get; set; }

    public int UsuarioId { get; set; }

    public int? OrganizacionId { get; set; }
}