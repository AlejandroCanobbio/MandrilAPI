using System.Threading.Tasks;
using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;
namespace MandrilAPI.Controllers;

[ApiController]
[Route("api/mandril/{mandrilId}/[controller]")]
public class HabilidadController : ControllerBase
{
    private readonly HabilidadService _habilidadService;
    public HabilidadController(HabilidadService mandrilService)
    {
        _habilidadService = mandrilService;
    }
    [HttpGet]
    public async Task<ActionResult<Habilidad>> GetHabilidades(string mandrilId)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        return Ok(mandril.Habilidades);
    }

    [HttpGet("{habilidadId}")]
    public async Task<ActionResult<Habilidad>> GetHabilidad(string mandrilId, int habilidadId)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);

        if (habilidad == null) {
            return NotFound(Mensajes.Habilidad.NotFound);
        }

        return Ok(habilidad);
        
    }

    [HttpPost]
    public async Task<ActionResult<Habilidad>> PostHabilidad([FromRoute] string mandrilId, [FromBody] HabilidadInsert habilidadInsert)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidadExiste = mandril.Habilidades?.FirstOrDefault(h => h.Nombre == habilidadInsert.Nombre);

        if (habilidadExiste != null)
        {
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }
        
        // var maxHabilidadId = mandril.Habilidades == null ? 0 : mandril.Habilidades.Max(x => x.Id);

        // var habilidadNueva = new Habilidad()
        // {
        //     Id = maxHabilidadId + 1,
        //     Nombre = habilidadInsert.Nombre,
        //     Potencia = habilidadInsert.Potencia
        // };

        var habilidad = await _habilidadService.InsertarHabilidad(mandrilId, habilidadInsert);

        return CreatedAtAction(nameof(GetHabilidad),
            new { mandrilId = mandrilId , habilidadId = habilidad.Id },
            habilidad
        );
    }

    [HttpPut("{habilidadId}")]
    public async Task<ActionResult<Habilidad>> PutHabilidad(string mandrilId, int habilidadId, HabilidadInsert habilidadInsert)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);

        if (habilidad == null) {
            return NotFound(Mensajes.Habilidad.NotFound);
        }

        var habilidadExiste = mandril.Habilidades?.FirstOrDefault(h => h.Nombre == habilidadInsert.Nombre);

        if (habilidadExiste != null)
        {
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }

        habilidad.Nombre = habilidadInsert.Nombre;
        habilidad.Potencia = habilidadInsert.Potencia;
        
        await _habilidadService.ActualizarHabilidad(mandrilId, habilidad, mandril);
        
        return NoContent();
    }

    [HttpDelete("{habilidadId}")]
    public async Task<ActionResult<Habilidad>> DeleteHabilidad(string mandrilId, int habilidadId)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);

        if (habilidad == null)
        {
            return NotFound(Mensajes.Habilidad.NotFound);
        }

        await _habilidadService.EliminarHabilidad(mandrilId, habilidadId);

        return NoContent();
    }
}
