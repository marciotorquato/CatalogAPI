using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Repository;

public interface ISearchRepository
{
    Task IndexAsync(Game game);
    Task<List<Game>> SearchAsync(string termo, int take = 20);
}
