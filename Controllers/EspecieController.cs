using GreenForest.Data;
using GreenForest.DTOs;
using GreenForest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecieController : ControllerBase
{
    private readonly GreenForestContext _context;

    public EspecieController(GreenForestContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _context.Especies.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var especie = await _context.Especies.FindAsync(id);
        if (especie == null) return NotFound();
        return Ok(especie);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(EspecieDTO dto)
    {
        var especie = new Especie
        {
            NombreComun = dto.NombreComun,
            NombreCientifico = dto.NombreCientifico,
            DistanciaSiembra = dto.DistanciaSiembra,
            ClimaIdeal = dto.ClimaIdeal,
            TemperaturaMinima = dto.TemperaturaMinima,
            TemperaturaMaxima = dto.TemperaturaMaxima,
            PrecipitacionMinima = dto.PrecipitacionMinima,
            PrecipitacionMaxima = dto.PrecipitacionMaxima,
            TipoSuelo = dto.TipoSuelo,
            ToleranciaSequía = dto.ToleranciaSequía
        };

        _context.Especies.Add(especie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = especie.Id }, especie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, EspecieDTO dto)
    {
        var especie = new Especie   
        {
        NombreComun = dto.NombreComun,
        NombreCientifico = dto.NombreCientifico,
        DistanciaSiembra = dto.DistanciaSiembra,
        ClimaIdeal = dto.ClimaIdeal,
        TemperaturaMinima = dto.TemperaturaMinima,
        TemperaturaMaxima = dto.TemperaturaMaxima,
        PrecipitacionMinima = dto.PrecipitacionMinima,
        PrecipitacionMaxima = dto.PrecipitacionMaxima,
        TipoSuelo = dto.TipoSuelo,
        ToleranciaSequía = dto.ToleranciaSequía
        };

        especie.NombreComun = dto.NombreComun;
        especie.NombreCientifico = dto.NombreCientifico;
        especie.DistanciaSiembra = dto.DistanciaSiembra;
        especie.ClimaIdeal = dto.ClimaIdeal;

        await _context.SaveChangesAsync();
        return Ok(especie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var especie = await _context.Especies.FindAsync(id);
        if (especie == null) return NotFound();

        _context.Especies.Remove(especie);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
