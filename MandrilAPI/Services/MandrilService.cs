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

    public async Task Crear(Mandril mandril)
    {
        await _mandriles.InsertOneAsync(mandril);
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
