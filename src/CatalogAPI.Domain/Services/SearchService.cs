using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using CatalogAPI.Domain.Interfaces.Services;

namespace CatalogAPI.Domain.Services;

public class SearchService : ISearchService
{
    private readonly ISearchRepository _repository;

    public SearchService(ISearchRepository repository)
        => _repository = repository;

    public Task IndexarAsync(Game game)
        => _repository.IndexAsync(game);

    public Task<List<Game>> BuscarAsync(string termo, int take = 20)
        => _repository.SearchAsync(termo, take);
}
