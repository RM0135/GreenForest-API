using GreenForest.DTOs;
using GreenForest.Models;

namespace GreenForest.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task<Usuario> CrearAsync(UsuarioDTO dto);

    Task<Usuario?> ActualizarAsync(int id, UsuarioDTO dto);

    Task<bool> EliminarAsync(int id);
}