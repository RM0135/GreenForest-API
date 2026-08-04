namespace GreenForest.DTOs;

public class ArbolDTO
{
    public int Id { get; set; }

    public DateTime FechaSiembra { get; set; }

    public string Estado { get; set; } = string.Empty;

    public decimal Altura { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public int ProyectoId { get; set; }

    public int EspecieId { get; set; }
}