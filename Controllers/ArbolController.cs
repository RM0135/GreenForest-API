using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArbolController : ControllerBase
{
    private readonly GreenForestContext _context;

    public ArbolController(GreenForestContext context)
    {
        _context = context;
    }

   [HttpGet]
public async Task<IActionResult> ObtenerTodos()
{
    var arboles = await _context.Arboles
        .Select(a => new ArbolDTO
        {
            Id = a.Id,
            FechaSiembra = a.FechaSiembra,
            Estado = a.Estado,
            Altura = a.Altura,
            Latitud = a.Latitud,
            Longitud = a.Longitud,
            ProyectoId = a.ProyectoId,
            EspecieId = a.EspecieId
        })
        .ToListAsync();

    return Ok(arboles);
}

    [HttpGet("{id}")]
public async Task<IActionResult> ObtenerPorId(int id)
{
    var arbol = await _context.Arboles
        .Where(a => a.Id == id)
        .Select(a => new ArbolDTO
        {
            Id = a.Id,
            FechaSiembra = a.FechaSiembra,
            Estado = a.Estado,
            Altura = a.Altura,
            Latitud = a.Latitud,
            Longitud = a.Longitud,
            ProyectoId = a.ProyectoId,
            EspecieId = a.EspecieId
        })
        .FirstOrDefaultAsync();

    if (arbol == null)
        return NotFound();

    return Ok(arbol);
}

    [HttpPost]
    public async Task<IActionResult> Crear(ArbolDTO dto)
    {
        var arbol = new Arbol
        {
            FechaSiembra = dto.FechaSiembra,
            Estado = string.IsNullOrWhiteSpace(dto.Estado) ? "Sembrado" : dto.Estado,
            Altura = dto.Altura,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            ProyectoId = dto.ProyectoId,
            EspecieId = dto.EspecieId
        };

        _context.Arboles.Add(arbol);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = arbol.Id }, arbol);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ArbolDTO dto)
    {
        var arbol = await _context.Arboles.FindAsync(id);
        if (arbol == null) return NotFound();

        arbol.FechaSiembra = dto.FechaSiembra;
        arbol.Estado = dto.Estado;
        arbol.Altura = dto.Altura;
        arbol.Latitud = dto.Latitud;
        arbol.Longitud = dto.Longitud;
        arbol.ProyectoId = dto.ProyectoId;
        arbol.EspecieId = dto.EspecieId;

        await _context.SaveChangesAsync();
        return Ok(arbol);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var arbol = await _context.Arboles.FindAsync(id);
        if (arbol == null) return NotFound();

        _context.Arboles.Remove(arbol);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
