using MandrilAPI.Helpers;
using MandrilAPI.Models;
using MongoDB.Driver;

namespace MandrilAPI.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    public UserService(IMongoDbContext mongoDbContext)
    {
        _users = mongoDbContext.GetCollection<User>("Users");
    }

    public async Task<List<User>> ObtenerTodos()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async Task<User?> ObtenerPorId(string sCorreo, string sClave)
    {
        //var filtro = Builders<User>.Filter.Eq(u => u.IdUser, id);
        var filtro = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Correo, sCorreo),
            Builders<User>.Filter.Exists(u => u.IdUser)
        );
        return await _users.Find(filtro).FirstOrDefaultAsync();
    }

    public async Task<User?> Crear(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }


}
