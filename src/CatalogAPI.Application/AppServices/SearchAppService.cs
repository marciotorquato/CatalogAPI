using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Game;
using CatalogAPI.Domain.Dtos.Responses.Search;
using CatalogAPI.Domain.Exceptions;
using CatalogAPI.Domain.Interfaces.Services;

namespace CatalogAPI.Application.AppServices;

public class SearchAppService : ISearchAppService
{
    private readonly ISearchService _searchService;

    public SearchAppService(ISearchService searchService)
        => _searchService = searchService;

    public async Task<List<GameSearchResponse>> Buscar(BuscarGameRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Termo))
        {
            throw new DomainException("O termo de busca é obrigatório.");
        }

        var resultados = await _searchService.BuscarAsync(request.Termo, request.Take);

        // A ordem já reflete a relevância (_score do Elasticsearch) — não
        // reordena aqui, só mapeia pra Response.
        return resultados.Select(g => new GameSearchResponse
        {
            Id = g.Id,
            Nome = g.Nome,
            Descricao = g.Descricao,
            Genero = g.Genero,
            Desenvolvedor = g.Desenvolvedor,
            Preco = g.Preco,
            DataRelease = g.DataRelease
        }).ToList();
    }
}
