using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MongoDB.Driver;

namespace MandrilAPI.Services;

public class HabilidadService
{
    private readonly IMongoCollection<Mandril> _mandriles;

    public HabilidadService(IMongoDbContext mongoDbContext)
    {
        _mandriles = mongoDbContext.GetCollection<Mandril>("Mandriles");
    }

    public async Task<Mandril?> ObtenerPorId(string id)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, id);
        return await _mandriles.Find(filtro).FirstOrDefaultAsync();
    }

    public async Task<List<Habilidad>> ObtenerHabilidadesPorMandrilId(string mandrilId)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId);
        var mandril = await _mandriles.Find(filtro).FirstOrDefaultAsync();

        return mandril?.Habilidades ?? new List<Habilidad>();
    }

    public async Task<List<Habilidad>> ObtenerHabilidadesPorId(string mandrilId, int habilidadId)
    {
        // var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId);
        // var mandril = await _mandriles.Find(filtro).FirstOrDefaultAsync();

        // var mandril?.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);

        // return mandril?.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        var filtro = Builders<Mandril>.Filter.And(
            Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId),
            Builders<Mandril>.Filter.ElemMatch(m => m.Habilidades, h => h.Id == habilidadId)
        );

        var resultado = await _mandriles.Find(filtro).FirstOrDefaultAsync();
        var habilidad = resultado?.Habilidades?.FirstOrDefault(h => h.Id == habilidadId);
        return habilidad != null ? new List<Habilidad> { habilidad } : new List<Habilidad>();
    }
    public async Task<Habilidad> InsertarHabilidad(string mandrilId, HabilidadInsert habilidadInsert)
    {
        var mandril = await ObtenerPorId(mandrilId);

        if (mandril == null)
        {
            throw new Exception(Mensajes.Mandril.NotFound);
        }

        var nuevaHabilidad = new Habilidad
        {
            Id = mandril.Habilidades?.Count > 0 ? mandril.Habilidades.Max(h => h.Id) + 1 : 1,
            Nombre = habilidadInsert.Nombre,
            Potencia = habilidadInsert.Potencia
        };

        mandril.Habilidades ??= new List<Habilidad>();
        mandril.Habilidades.Add(nuevaHabilidad);

        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId);
        await _mandriles.ReplaceOneAsync(filtro, mandril);

        return nuevaHabilidad;
    }

    public async Task<Habilidad> ActualizarHabilidad(string mandrilId, Habilidad habilidadInsert, Mandril mandril)
    {

        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId);
        await _mandriles.ReplaceOneAsync(filtro, mandril);

        return habilidadInsert;
    }
    public async Task EliminarHabilidad(string mandrilId, int habilidadId)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, mandrilId);
        var actualizacion = Builders<Mandril>.Update.PullFilter(
            m => m.Habilidades,
            h => h.Id == habilidadId
        );

        var resultado = await _mandriles.UpdateOneAsync(filtro, actualizacion);
    }
}
