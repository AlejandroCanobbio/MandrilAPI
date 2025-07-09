using System.Threading.Tasks;
using AutoMapper;
using MandrilAPI.DTOs;
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
    private readonly IMapper _mapper;
    public HabilidadController(HabilidadService mandrilService, IMapper mapper)
    {
        _habilidadService = mandrilService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<HabilidadDto>> GetHabilidades(string mandrilId)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }
        var habilidadDtos = _mapper.Map<IEnumerable<HabilidadDto>>(mandril.Habilidades);
        return Ok(habilidadDtos);
    }

    [HttpGet("{habilidadId}")]
    public async Task<ActionResult<HabilidadDto>> GetHabilidad(string mandrilId, int habilidadId)
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
        var habilidadDto = _mapper.Map<HabilidadDto>(habilidad);
        return Ok(habilidadDto);
        
    }

    [HttpPost]
    public async Task<ActionResult<HabilidadDto>> PostHabilidad([FromRoute] string mandrilId, [FromBody] HabilidadCreateDto  habilidadDto)
    {
        var mandril = await _habilidadService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        var habilidadExiste = mandril.Habilidades?.FirstOrDefault(h => h.Nombre == habilidadDto.Nombre);

        if (habilidadExiste != null)
        {
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }

        var habilidadNueva = _mapper.Map<Habilidad>(habilidadDto);
        habilidadNueva.Id = mandril.Habilidades?.Count > 0 ? mandril.Habilidades.Max(h => h.Id) + 1 : 1;
        var habilidad = await _habilidadService.InsertarHabilidad(mandrilId, habilidadNueva, mandril);
        
        var habilidadDtoResponse = _mapper.Map<HabilidadDto>(habilidad);

        return CreatedAtAction(nameof(GetHabilidad),
            new { mandrilId = mandrilId, habilidadId = habilidad.Id },
            habilidadDtoResponse
        );
    }

    [HttpPut("{habilidadId}")]
    public async Task<ActionResult<HabilidadDto>> PutHabilidad(string mandrilId, int habilidadId, HabilidadCreateDto  habilidadDto)
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

        var habilidadExiste = mandril.Habilidades?.FirstOrDefault(h => h.Nombre == habilidadDto.Nombre);

        if (habilidadExiste != null)
        {
            return BadRequest(Mensajes.Habilidad.NombreExistente);
        }
        habilidad.Nombre = habilidadDto.Nombre;
        habilidad.Potencia = habilidadDto.Potencia;
        
        
        await _habilidadService.ActualizarHabilidad(mandrilId, habilidad, mandril);
        
        return NoContent();
    }

    [HttpDelete("{habilidadId}")]
    public async Task<ActionResult<HabilidadDto>> DeleteHabilidad(string mandrilId, int habilidadId)
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
