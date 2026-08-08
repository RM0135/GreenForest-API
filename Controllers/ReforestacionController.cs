using GreenForest.AI;
using GreenForest.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GreenForest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReforestacionController : ControllerBase
    {
        private readonly GeminiService _geminiService;

        public ReforestacionController(
            GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        // ============================================================
        // RECOMENDACIÓN DE REFORESTACIÓN
        // ============================================================

        [HttpPost("recomendar")]
        public async Task<ActionResult<RecomendacionReforestacionDTO>>
            Recomendar([FromBody] ReforestacionRequestDTO request)
        {
            try
            {
                var resultado =
                    await _geminiService.RecomendarReforestacion(
                        request.ProyectoId,
                        request.Temperatura,
                        request.Precipitacion,
                        request.Humedad,
                        request.TipoSuelo
                    );

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}