using GreenForest.AI;
using Microsoft.AspNetCore.Mvc;

namespace GreenForest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IAController : ControllerBase
{
    private readonly GeminiService _geminiService;

    public IAController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<GeminiResponse>> Chat(GeminiRequest request)
    {
        var respuesta = await _geminiService.PreguntarLibre(request.Pregunta);

        return Ok(new GeminiResponse
        {
            Respuesta = respuesta
        });
    }
}