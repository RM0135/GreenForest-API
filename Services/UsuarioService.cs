using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Services;

public class UsuarioService
{
    private readonly GreenForestContext _context;

    public UsuarioService(GreenForestContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ObtenerTodos()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> ObtenerPorId(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> Crear(UsuarioDTO dto)
    {
        Usuario usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Password = dto.Password,
            Rol = dto.Rol
        };

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> Actualizar(int id, UsuarioDTO dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return null;

        usuario.Nombre = dto.Nombre;
        usuario.Apellido = dto.Apellido;
        usuario.Correo = dto.Correo;
        usuario.Password = dto.Password;
        usuario.Rol = dto.Rol;

        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<bool> Eliminar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();

        return true;
    }
}