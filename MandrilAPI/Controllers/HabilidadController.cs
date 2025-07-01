using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MandrilAPI.Controllers;

[ApiController]
[Route("api/mandril/{mandrilId}/[controller]")]
public class HabilidadController : ControllerBase
{
    [HttpGet]
    public ActionResult<Habilidad> GetHabilidades(int mandrilId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        return Ok(mandril.Habilidades);
    }

    [HttpGet("{habilidadId}")]
    public ActionResult<Habilidad> GetHabilidad(int mandrilId, int habilidadId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

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
    public ActionResult<Habilidad> PostHabilidad([FromRoute] int mandrilId, [FromBody] HabilidadInsert habilidadInsert)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidadExiste = mandril.Habilidades?.FirstOrDefault(h => h.Nombre == habilidadInsert.Nombre);

        if (habilidadExiste != null)
        {
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }
        
        var maxHabilidadId = mandril.Habilidades == null ? 0 : mandril.Habilidades.Max(x => x.Id);

        var habilidadNueva = new Habilidad()
        {
            Id = maxHabilidadId + 1,
            Nombre = habilidadInsert.Nombre,
            Potencia = habilidadInsert.Potencia
        };

        mandril.Habilidades?.Add(habilidadNueva);

        return CreatedAtAction(nameof(GetHabilidad),
            new { mandrilId = mandrilId , habilidadId = habilidadNueva.Id },
            habilidadNueva
        );
    }

    [HttpPut("{habilidadId}")]
    public ActionResult<Habilidad> PutHabilidad(int mandrilId, int habilidadId, HabilidadInsert habilidadInsert)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

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
        
        return NoContent();
    }

    [HttpDelete("{habilidadId}")]
    public ActionResult<Habilidad> DeleteHabilidad(int mandrilId, int habilidadId)
    {
        var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidad = mandril.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);

        if (habilidad == null)
        {
            return NotFound(Mensajes.Habilidad.NotFound);
        }

        mandril.Habilidades?.Remove(habilidad);

        return NoContent();
    }
}
