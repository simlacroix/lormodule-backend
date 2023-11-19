using LoRModule.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoRModule.Repositories;

public abstract class Repository
{
    protected readonly IMongoDatabase MongoDatabase;

    protected Repository(IOptions<LoRDatabaseSettings> lorDatabaseSettings)
    {
        var mongoClient = new MongoClient(lorDatabaseSettings.Value.ConnectionString);
        MongoDatabase = mongoClient.GetDatabase(lorDatabaseSettings.Value.DatabaseName);
    }
}