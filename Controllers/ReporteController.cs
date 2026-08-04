using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReporteController : ControllerBase
{
    private readonly GreenForestContext _context;

    public ReporteController(GreenForestContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _context.Reportes.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var reporte = await _context.Reportes
            .Include(r => r.Proyecto)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reporte == null) return NotFound();
        return Ok(reporte);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ReporteDTO dto)
    {
        var reporte = new Reporte
        {
            FechaGeneracion = dto.FechaGeneracion,
            Descripcion = dto.Descripcion,
            CantidadArboles = dto.CantidadArboles,
            Observaciones = dto.Observaciones,
            ProyectoId = dto.ProyectoId
        };

        _context.Reportes.Add(reporte);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = reporte.Id }, reporte);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ReporteDTO dto)
    {
        var reporte = await _context.Reportes.FindAsync(id);
        if (reporte == null) return NotFound();

        reporte.FechaGeneracion = dto.FechaGeneracion;
        reporte.Descripcion = dto.Descripcion;
        reporte.CantidadArboles = dto.CantidadArboles;
        reporte.Observaciones = dto.Observaciones;
        reporte.ProyectoId = dto.ProyectoId;

        await _context.SaveChangesAsync();
        return Ok(reporte);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var reporte = await _context.Reportes.FindAsync(id);
        if (reporte == null) return NotFound();

        _context.Reportes.Remove(reporte);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
