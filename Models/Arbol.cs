using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenForest.Models;

public class Arbol
{
    public int Id { get; set; }

    public DateTime FechaSiembra { get; set; }

    public string Estado { get; set; } = "Sembrado";

    public decimal Altura { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    // Llave foránea: proyecto al que pertenece
    public int ProyectoId { get; set; }

    [ForeignKey("ProyectoId")]
    public Proyecto? Proyecto { get; set; }

    // Llave foránea: especie plantada
    public int EspecieId { get; set; }

    [ForeignKey("EspecieId")]
    public Especie? Especie { get; set; }
}