using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Rating;
using CatalogAPI.Domain.Dtos.Responses.Rating;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Exceptions;
using CatalogAPI.Domain.Interfaces.Services;

namespace CatalogAPI.Application.AppServices;

public class GameRatingAppService : IGameRatingAppService
{
    private readonly IGameRatingService _ratingService;
    private readonly IGameService _gameService;

    public GameRatingAppService(IGameRatingService ratingService, IGameService gameService)
    {
        _ratingService = ratingService;
        _gameService = gameService;
    }

    public async Task<GameRatingResponse> AvaliarGameAsync(Guid gameId, AvaliarGameRequest request)
    {
        var game = _gameService.GetById(gameId);
        if (game is null)
            throw new NotFoundException("Game nao encontrado.");

        if (request.Nota is < 1 or > 5)
            throw new DomainException("Nota deve ser entre 1 e 5.");

        var rating = GameRating.Criar(gameId, request.UsuarioId, request.Nota, request.Comentario);
        var saved = await _ratingService.AvaliarAsync(rating);
        return MapToResponse(saved);
    }

    public async Task<GameRatingSummaryResponse> ObterAvaliacoesAsync(Guid gameId)
    {
        var ratings = await _ratingService.ListarPorGameAsync(gameId);
        var media = await _ratingService.MediaNotaAsync(gameId);
        return new GameRatingSummaryResponse
        {
            GameId = gameId,
            MediaNota = Math.Round(media, 2),
            TotalAvaliacoes = ratings.Count,
            Avaliacoes = ratings.Select(MapToResponse).ToList()
        };
    }

    private static GameRatingResponse MapToResponse(GameRating r) => new()
    {
        Id = r.Id,
        GameId = r.GameId,
        UserId = r.UserId,
        Nota = r.Nota,
        Comentario = r.Comentario,
        CriadoEm = r.CriadoEm
    };
}
