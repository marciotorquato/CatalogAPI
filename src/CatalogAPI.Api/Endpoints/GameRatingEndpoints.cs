using CatalogAPI.Api.Helpers;
using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Rating;
using CatalogAPI.Domain.Dtos.Responses.Rating;

namespace CatalogAPI.Api.Endpoints;

public static class GameRatingEndpoints
{
    public static void MapGameRatings(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Game/{gameId:guid}/ratings").WithTags("GameRatings");

        app.MapPost("/", async (Guid gameId, AvaliarGameRequest request, IGameRatingAppService service) =>
        {
            var result = await service.AvaliarGameAsync(gameId, request);
            return ApiResponses.Created($"/api/Game/{gameId}/ratings", result, "Avaliacao registrada com sucesso.");
        })
        .RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("AvaliarGame")
        .Produces<GameRatingResponse>(201)
        .Produces(400)
        .Produces(404);

        app.MapGet("/", async (Guid gameId, IGameRatingAppService service) =>
        {
            var result = await service.ObterAvaliacoesAsync(gameId);
            return ApiResponses.Ok(result, "Avaliacoes listadas com sucesso.");
        })
        .RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("ObterAvaliacoesGame")
        .Produces<GameRatingSummaryResponse>(200);
    }
}
