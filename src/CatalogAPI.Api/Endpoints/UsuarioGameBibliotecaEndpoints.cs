using CatalogAPI.Api.Filters;
using CatalogAPI.Api.Helpers;
using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.UsuarioGameBiblioteca;
using CatalogAPI.Domain.Dtos.Responses.UsuarioGameBiblioteca;

namespace CatalogAPI.Api.Endpoints;

public static class UsuarioGameBibliotecaEndpoints
{
    public static void MapUsuarioGameBiblioteca(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/biblioteca").WithTags("Biblioteca");

        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            var biblioteca = await bibliotecaService.ListarPorUsuario(usuarioId);
            return ApiResponses.Ok(biblioteca, "Biblioteca listada com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("ListarBibliotecaDoUsuario")
        .Produces<List<BibliotecaResponse>>(200);


        app.MapPost("Comprar/", async (Guid usuarioId, ComprarGameRequest request, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            request = request with { UsuarioId = usuarioId };
            var (biblioteca, sucesso, errorMessage) = await bibliotecaService.ComprarGame(request);
            if (!sucesso)
            {
                return ApiResponses.BadRequest("game", errorMessage ?? "Não foi possível comprar o jogo.");
            }
            return ApiResponses.Created($"/api/usuarios/{usuarioId}/biblioteca/{biblioteca!.Id}", biblioteca, "Jogo comprado e adicionado à biblioteca com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .AddEndpointFilter<ValidationEndpointFilter<ComprarGameRequest>>()
        .WithName("ComprarGame")
        .Produces<BibliotecaResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarBibliotecaRequest request, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            if (id != request.Id)
            {
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }
            request = request with { UsuarioId = usuarioId };
            var (biblioteca, sucesso) = await bibliotecaService.Atualizar(request);
            if (!sucesso || biblioteca == null)
            {
                return ApiResponses.NotFound("biblioteca", "Item da biblioteca não encontrado ou não pertence ao usuário.");
            }
            return ApiResponses.Ok(biblioteca, "Item da biblioteca atualizado com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarBibliotecaRequest>>()
        .WithName("AtualizarBiblioteca")
        .Produces<BibliotecaResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            var sucesso = await bibliotecaService.Deletar(id, usuarioId);
            if (!sucesso)
            {
                return ApiResponses.NotFound("biblioteca", "Item da biblioteca não encontrado ou não pertence ao usuário.");
            }
            return ApiResponses.OkMessage("Jogo removido da biblioteca com sucesso.");
        })
        //.RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("DeletarDaBiblioteca")
        .Produces(200)
        .Produces(404);
    }
}