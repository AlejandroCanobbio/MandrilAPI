using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MandrilAPI.Helpers;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string name);
}

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}