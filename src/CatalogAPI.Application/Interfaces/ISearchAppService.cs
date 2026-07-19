using CatalogAPI.Domain.Dtos.Request.Game;
using CatalogAPI.Domain.Dtos.Responses.Search;

namespace CatalogAPI.Application.Interfaces;

public interface ISearchAppService
{
    Task<List<GameSearchResponse>> Buscar(BuscarGameRequest request);
}
