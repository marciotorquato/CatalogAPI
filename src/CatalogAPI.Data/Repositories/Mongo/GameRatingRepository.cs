using CatalogAPI.Data.Mongo;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using MongoDB.Driver;

namespace CatalogAPI.Data.Repositories.Mongo;

public class GameRatingRepository : IGameRatingRepository
{
    private readonly IMongoCollection<GameRating> _collection;

    public GameRatingRepository(MongoDbContext context)
        => _collection = context.GetCollection<GameRating>("game_ratings");

    public async Task<GameRating> InserirAsync(GameRating rating)
    {
        await _collection.InsertOneAsync(rating);
        return rating;
    }

    public async Task<List<GameRating>> ListarPorGameAsync(Guid gameId)
        => await _collection
            .Find(r => r.GameId == gameId)
            .SortByDescending(r => r.CriadoEm)
            .ToListAsync();

    public async Task<double> MediaNotaAsync(Guid gameId)
    {
        var ratings = await ListarPorGameAsync(gameId);
        return ratings.Count == 0 ? 0 : ratings.Average(r => r.Nota);
    }
}
