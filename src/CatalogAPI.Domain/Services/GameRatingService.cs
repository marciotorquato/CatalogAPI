using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using CatalogAPI.Domain.Interfaces.Services;

namespace CatalogAPI.Domain.Services;

public class GameRatingService : IGameRatingService
{
    private readonly IGameRatingRepository _repository;

    public GameRatingService(IGameRatingRepository repository)
        => _repository = repository;

    public Task<GameRating> AvaliarAsync(GameRating rating)
        => _repository.InserirAsync(rating);

    public Task<List<GameRating>> ListarPorGameAsync(Guid gameId)
        => _repository.ListarPorGameAsync(gameId);

    public Task<double> MediaNotaAsync(Guid gameId)
        => _repository.MediaNotaAsync(gameId);
}
