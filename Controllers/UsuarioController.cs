using GreenForest.DTOs;
using GreenForest.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await _service.ObtenerPorId(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(UsuarioDTO dto)
    {
        var usuario = await _service.Crear(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, UsuarioDTO dto)
    {
        var usuario = await _service.Actualizar(id, dto);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _service.Eliminar(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
