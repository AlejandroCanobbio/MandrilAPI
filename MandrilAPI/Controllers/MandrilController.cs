using System.Threading.Tasks;
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
    public MandrilController(MandrilService mandrilService)
    {
        _mandrilService = mandrilService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mandril>>> GetMandriles()
    {
        //return Ok(MandrilDataStore.Current.Mandriles);
        var mandriles = await _mandrilService.ObtenerTodos();
        return Ok(mandriles);
    }

    [HttpGet("{mandrilId}")]
    public async Task<ActionResult<Mandril>> GetMandril(string mandrilId)
    {
        //var mandril = MandrilDataStore.Current.Mandriles.FirstOrDefault(x => x.Id == mandrilId);

        var mandril = await _mandrilService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }
        return Ok(mandril);
    }

    [HttpPost]
    public async Task<ActionResult<Mandril>> PostMandril(Mandrilinsert mandrilinsert)
    {
        var mandrilNuevo = new Mandril()
        {
            Nombre = mandrilinsert.Nombre,
            Apellido = mandrilinsert.Apellido
        };

        //MandrilDataStore.Current.Mandriles.Add(mandrilNuevo);
        var mandrilCrear = await _mandrilService.Crear(mandrilNuevo);
        return CreatedAtAction(nameof(GetMandril),
            new { mandrilId = mandrilCrear?.Id },
            mandrilNuevo
        );
    }

    [HttpPut("{mandrilId}")]
    public ActionResult<Mandril> PutMandril([FromRoute] string mandrilId, [FromBody] Mandrilinsert mandrilinsert)
    { 
        var mandril = _mandrilService.ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            return NotFound(Mensajes.Mandril.NotFound);
        }
        // mandril.Nombre = mandrilinsert.Nombre;
        // mandril.Apellido = mandrilinsert.Apellido;
        var mandrilActualizado = _mandrilService.Actualizar(mandrilId, new Mandril
        {
            Nombre = mandrilinsert.Nombre,
            Apellido = mandrilinsert.Apellido
        });


        return NoContent();
    }

    [HttpDelete("{mandrilId}")]
    public ActionResult<Mandril> DeleteMandril(string mandrilId)
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
