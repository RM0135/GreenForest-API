namespace GreenForest.DTOs;

public class EspecieDTO
{
    public string NombreComun { get; set; } = string.Empty;

    public string NombreCientifico { get; set; } = string.Empty;

    public decimal DistanciaSiembra { get; set; }

    public string ClimaIdeal { get; set; } = string.Empty;
}
