using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProyectoController : ControllerBase
{
    private readonly GreenForestContext _context;

    public ProyectoController(GreenForestContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _context.Proyectos
            .Include(p => p.Usuario)
            .Include(p => p.Organizacion)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var proyecto = await _context.Proyectos
            .Include(p => p.Usuario)
            .Include(p => p.Organizacion)
            .Include(p => p.Arboles)
            .Include(p => p.Reportes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proyecto == null) return NotFound();
        return Ok(proyecto);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ProyectoDTO dto)
    {
        var proyecto = new Proyecto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            MetaArboles = dto.MetaArboles,
            MetaHectareas = dto.MetaHectareas,
            UsuarioId = dto.UsuarioId,
            OrganizacionId = dto.OrganizacionId
        };

        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = proyecto.Id }, proyecto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ProyectoDTO dto)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null) return NotFound();

        proyecto.Nombre = dto.Nombre;
        proyecto.Descripcion = dto.Descripcion;
        proyecto.FechaInicio = dto.FechaInicio;
        proyecto.FechaFin = dto.FechaFin;
        proyecto.MetaArboles = dto.MetaArboles;
        proyecto.MetaHectareas = dto.MetaHectareas;
        proyecto.UsuarioId = dto.UsuarioId;
        proyecto.OrganizacionId = dto.OrganizacionId;

        await _context.SaveChangesAsync();
        return Ok(proyecto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null) return NotFound();

        _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
