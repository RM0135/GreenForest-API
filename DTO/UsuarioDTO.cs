using System.ComponentModel.DataAnnotations;

namespace GreenForest.DTOs;

public class UsuarioDTO
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Rol { get; set; } = "Operario";
}