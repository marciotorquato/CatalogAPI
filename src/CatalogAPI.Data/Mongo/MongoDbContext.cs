using CatalogAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CatalogAPI.Data.Mongo;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    static MongoDbContext()
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("CatalogApiConventions", pack, _ => true);

        BsonClassMap.TryRegisterClassMap<GameRating>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            cm.MapMember(x => x.GameId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            cm.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });
    }

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDB:ConnectionString"]
            ?? "mongodb://root:root@mongodb:27017";
        var databaseName = configuration["MongoDB:DatabaseName"]
            ?? "MS_CatalogAPI";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
        => _database.GetCollection<T>(name);
}
