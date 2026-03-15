using CatalogAPI.Api.Filters;
using CatalogAPI.Api.Helpers;
using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Game;
using CatalogAPI.Domain.Dtos.Responses.Game;

namespace CatalogAPI.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Game").WithTags("Game");

        app.MapPost("Cadastrar/", async (CadastrarGameRequest request, IGameAppService gameService) =>
        {
            var result = await gameService.Cadastrar(request);
            if (result == null)
            {
                return ApiResponses.Problem("Erro ao cadastrar o jogo.");
            }
            return ApiResponses.Created($"/api/Game/{result.Id}", result, "Jogo cadastrado com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("administrador"))
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarGameRequest>>()
        .WithName("CadastrarGame")
        .Produces<GameResponse>(201)
        .Produces(400)
        .Produces(500);


        app.MapGet("BuscarPorId/{id}", (Guid id, IGameAppService gameService) =>
        {
            var result = gameService.BuscarPorId(id);
            if (result == null)
            {
                return ApiResponses.NotFound("game", "Jogo não encontrado.");
            }
            return ApiResponses.Ok(result, "Jogo encontrado com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("BuscarGamePorId")
        .Produces<GameResponse>(200)
        .Produces(404);


        app.MapGet("ListarGames", async ([AsParameters] ListarGamesPaginadoRequest request, IGameAppService gameService) =>
        {
            var result = await gameService.ListarGamesPaginado(request);
            return ApiResponses.Ok(result, "Jogos listados com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .AddEndpointFilter<ValidationEndpointFilter<ListarGamesPaginadoRequest>>()
        .WithName("ListarGamesPaginado")
        .Produces<ListarGamesPaginadoResponse>(200)
        .Produces(400);


        app.MapPut("AtualizarGame/{id:guid}", async (Guid id, AtualizarGameRequest request, IGameAppService gameService) =>
        {
            if (id != request.Id)
            {
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }
            var (game, sucesso) = await gameService.AtualizarGame(request);
            if (!sucesso || game == null)
            {
                return ApiResponses.NotFound("game", "Jogo não encontrado ou não foi possível atualizar.");
            }
            return ApiResponses.Ok(game, "Jogo atualizado com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("administrador"))
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarGameRequest>>()
        .WithName("AtualizarGame")
        .Produces<GameResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}