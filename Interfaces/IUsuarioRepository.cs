using GreenForest.Models;

namespace GreenForest.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ObtenerTodosAsync();

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task<Usuario> CrearAsync(Usuario usuario);

    Task<Usuario?> ActualizarAsync(Usuario usuario);

    Task<bool> EliminarAsync(int id);
}