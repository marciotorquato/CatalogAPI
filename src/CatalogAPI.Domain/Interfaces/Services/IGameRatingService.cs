using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Services;

public interface IGameRatingService
{
    Task<GameRating> AvaliarAsync(GameRating rating);
    Task<List<GameRating>> ListarPorGameAsync(Guid gameId);
    Task<double> MediaNotaAsync(Guid gameId);
}
