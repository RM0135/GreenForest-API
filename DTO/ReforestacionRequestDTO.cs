namespace GreenForest.DTO
{
    public class ReforestacionRequestDTO
    {
        public int ProyectoId { get; set; }

        public decimal Temperatura { get; set; }

        public decimal Precipitacion { get; set; }

        public decimal Humedad { get; set; }

        public string TipoSuelo { get; set; } = string.Empty;
    }
}