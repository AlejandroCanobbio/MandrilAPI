using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MongoDB.Driver;

namespace MandrilAPI.Services;

public class MandrilService
{
    private readonly IMongoCollection<Mandril> _mandriles;

    public MandrilService(IMongoDbContext mongoDbContext)
    {
        _mandriles = mongoDbContext.GetCollection<Mandril>("Mandriles");
    }

    // Example method to get a collection
    public async Task<List<Mandril>> ObtenerTodos()
    {
        return await _mandriles.Find(_ => true).ToListAsync();
    }

    // public async Task<Mandril> ObtenerPorId(string id)
    // {
    //     var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, id);
    //     return await _mandriles.Find(filtro).ToListAsync();
    // }

    public async Task<Mandril?> ObtenerPorId(string id)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, id);
        return await _mandriles.Find(filtro).FirstOrDefaultAsync();
    }

    public async Task<Mandril?> Crear(Mandril mandril)
    {
        await _mandriles.InsertOneAsync(mandril);
        return mandril;
    }

    public async Task Actualizar(string id, Mandril mandrilActualizado)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, id);

        var update = Builders<Mandril>.Update
            .Set(m => m.Nombre, mandrilActualizado.Nombre)
            .Set(m => m.Apellido, mandrilActualizado.Apellido);
        await _mandriles.UpdateOneAsync(filtro, update);
    }

    public async Task Eliminar(string id)
    {
        var filtro = Builders<Mandril>.Filter.Eq(m => m.Id, id);
        await _mandriles.DeleteOneAsync(filtro);
    }

    // Opcional: método para cargar los mandriles precargados
    public async Task CargarMandrilesDePrueba()
    {
        if ((await _mandriles.CountDocumentsAsync(_ => true)) == 0)
        {
            var iniciales = new List<Mandril>
            {
                new() {
                    Nombre = "Mini Mandril",
                    Apellido = "Rodriguez",
                    Habilidades = new List<Habilidad> {
                        new() { Nombre = "Saltar", Potencia = Habilidad.EPotencia.Moderado }
                    }
                },
                new() {
                    Nombre = "SuperMandril",
                    Apellido = "Fernandez",
                    Habilidades = new List<Habilidad> {
                        new() { Nombre = "Saltar", Potencia = Habilidad.EPotencia.Moderado },
                        new() { Nombre = "Caminar", Potencia = Habilidad.EPotencia.Intenso },
                        new() { Nombre = "Correr", Potencia = Habilidad.EPotencia.RePotente }
                    }
                },
                new() {
                    Nombre = "Megamandril",
                    Apellido = "Legrand",
                    Habilidades = new List<Habilidad> {
                        new() { Nombre = "Nadar", Potencia = Habilidad.EPotencia.Intenso },
                        new() { Nombre = "Correr", Potencia = Habilidad.EPotencia.Extremo },
                        new() { Nombre = "Vomitar", Potencia = Habilidad.EPotencia.RePotente }
                    }
                }
            };

            await _mandriles.InsertManyAsync(iniciales);
        }
    }
}
