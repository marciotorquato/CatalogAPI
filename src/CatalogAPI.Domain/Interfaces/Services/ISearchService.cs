using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Services;

public interface ISearchService
{
    Task IndexarAsync(Game game);
    Task<List<Game>> BuscarAsync(string termo, int take = 20);
}
