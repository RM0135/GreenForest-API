namespace GreenForest.DTOs; 
public class EspecieDTO 
{ 
public string NombreComun { get; set; } = string.Empty;
 public string NombreCientifico { get; set; } = string.Empty; 
 public decimal DistanciaSiembra { get; set; } 
 public string ClimaIdeal { get; set; } = string.Empty;
  public decimal TemperaturaMinima { get; set; } 
  public decimal TemperaturaMaxima { get; set; } 
  public decimal PrecipitacionMinima { get; set; }
   public decimal PrecipitacionMaxima { get; set; } 
   public string TipoSuelo { get; set; } = string.Empty; 
   public string ToleranciaSequía { get; set; } = string.Empty; 

}