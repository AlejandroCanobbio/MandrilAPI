using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MandrilAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUser { get; set; } = null!;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}
