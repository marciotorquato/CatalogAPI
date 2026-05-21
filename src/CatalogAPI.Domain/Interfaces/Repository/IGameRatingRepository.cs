using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Repository;

public interface IGameRatingRepository
{
    Task<GameRating> InserirAsync(GameRating rating);
    Task<List<GameRating>> ListarPorGameAsync(Guid gameId);
    Task<double> MediaNotaAsync(Guid gameId);
}
