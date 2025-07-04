using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MandrilAPI.Models;

public class Mandril
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public List<Habilidad>? Habilidades { get; set; } = new List<Habilidad>();
}
