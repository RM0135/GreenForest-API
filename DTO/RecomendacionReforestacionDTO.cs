namespace GreenForest.DTO;

public class RecomendacionReforestacionDTO
{
    public string EspecieRecomendada { get; set; } = string.Empty;

    public string NombreCientifico { get; set; } = string.Empty;

    public decimal DistanciaSiembra { get; set; }

    public string ClimaIdeal { get; set; } = string.Empty;

    public decimal Compatibilidad { get; set; }

    public int ArbolesEstimados { get; set; }

    public int MetaArboles { get; set; }

    public int ArbolesActuales { get; set; }

    public int ArbolesFaltantes { get; set; }

    public decimal PorcentajeCumplimiento { get; set; }

    public bool MetaAlcanzada { get; set; }

    public string Justificacion { get; set; } = string.Empty;

    public List<string> Recomendaciones { get; set; } = new();
}