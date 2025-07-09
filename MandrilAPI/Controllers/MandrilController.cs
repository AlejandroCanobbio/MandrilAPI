using System.Threading.Tasks;
using AutoMapper;
using MandrilAPI.DTOs;
using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MandrilAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MandrilAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MandrilController : ControllerBase
{
    private readonly MandrilService _mandrilService;
    private readonly IMapper _mapper;
    public MandrilController(MandrilService mandrilService, IMapper mapper)
    {
        _mandrilService = mandrilService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MandrilResponseDto>>> GetMandriles()
    {
        //return Ok(MandrilDataStore.Current.Mandriles);
        var mandriles = await _mandrilService.ObtenerTodos();
        var mandrilesDto = _mapper.Map<IEnumerable<MandrilResponseDto>>(mandriles);
        return Ok(mandrilesDto);
    }

    [HttpGet("{mandrilId}")]
    public async Task<ActionResult<MandrilResponseDto>> GetMandril(string mandrilId)
    {
        //var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

        var mandril = await _mandrilService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }
        var mandrilIdGet = _mapper.Map<MandrilResponseDto>(mandril);
        return Ok(mandrilIdGet);
    }

    [HttpPost]
    public async Task<ActionResult<MandrilResponseDto>> PostMandril(MandrilCreateDto mandrilCreateDto)
    {
        var mandrilNuevo = _mapper.Map<Mandril>(mandrilCreateDto);

        //MandrilDataStore.Current.Mandriles.Add(mandrilNuevo);
        var mandrilCreado = await _mandrilService.Crear(mandrilNuevo);

        var mandrilResponse = _mapper.Map<MandrilResponseDto>(mandrilCreado);
        return CreatedAtAction(nameof(GetMandril),
            new { mandrilId = mandrilResponse?.Id },
            mandrilNuevo
        );
    }

    [HttpPut("{mandrilId}")]
    public ActionResult<MandrilResponseDto> PutMandril([FromRoute] string mandrilId, [FromBody] MandrilCreateDto mandrilCreateDto)
    { 
        var mandril = _mandrilService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }
        var mandrilinsert = _mapper.Map<Mandril>(mandrilCreateDto);
        // mandril.Nombre = mandrilinsert.Nombre;
        // mandril.Apellido = mandrilinsert.Apellido;
        var mandrilActualizado = _mandrilService.Actualizar(mandrilId, mandrilinsert);


        return NoContent();
    }

    [HttpDelete("{mandrilId}")]
    public ActionResult<MandrilResponseDto> DeleteMandril(string mandrilId)
    { 
        var mandril = _mandrilService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }

        //MandrilDataStore.Current.Mandriles.Remove(mandril);
        var mandrilEliminar = _mandrilService.Eliminar(mandrilId);
        return NoContent();
    }
}
