using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizacionController : ControllerBase
{
    private readonly GreenForestContext _context;

    public OrganizacionController(GreenForestContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _context.Organizaciones.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var organizacion = await _context.Organizaciones.FindAsync(id);
        if (organizacion == null) return NotFound();
        return Ok(organizacion);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(OrganizacionDTO dto)
    {
        var organizacion = new Organizacion
        {
            Nombre = dto.Nombre,
            Tipo = dto.Tipo,
            Telefono = dto.Telefono,
            Correo = dto.Correo
        };

        _context.Organizaciones.Add(organizacion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = organizacion.Id }, organizacion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, OrganizacionDTO dto)
    {
        var organizacion = await _context.Organizaciones.FindAsync(id);
        if (organizacion == null) return NotFound();

        organizacion.Nombre = dto.Nombre;
        organizacion.Tipo = dto.Tipo;
        organizacion.Telefono = dto.Telefono;
        organizacion.Correo = dto.Correo;

        await _context.SaveChangesAsync();
        return Ok(organizacion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var organizacion = await _context.Organizaciones.FindAsync(id);
        if (organizacion == null) return NotFound();

        _context.Organizaciones.Remove(organizacion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
