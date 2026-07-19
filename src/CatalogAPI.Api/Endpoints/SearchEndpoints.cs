using CatalogAPI.Api.Helpers;
using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Game;

namespace CatalogAPI.Api.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearch(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Search").WithTags("Search");

        // GET /api/Search?termo=elden&take=20
        // Fuzzy Search (tolerância a erros de digitação) + ordenação por
        // relevância, via Elasticsearch.
        app.MapGet("/", async ([AsParameters] BuscarGameRequest request, ISearchAppService searchService) =>
        {
            var result = await searchService.Buscar(request);
            return ApiResponses.Ok(result, "Busca realizada com sucesso.");
        })
        .RequireAuthorization(policy => policy.RequireRole("usuario"))
        .WithName("BuscarGames")
        .Produces(200)
        .Produces(400);
    }
}
