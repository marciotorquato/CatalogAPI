using CatalogAPI.Domain.Dtos.Request.Rating;
using CatalogAPI.Domain.Dtos.Responses.Rating;

namespace CatalogAPI.Application.Interfaces;

public interface IGameRatingAppService
{
    Task<GameRatingResponse> AvaliarGameAsync(Guid gameId, AvaliarGameRequest request);
    Task<GameRatingSummaryResponse> ObterAvaliacoesAsync(Guid gameId);
}
