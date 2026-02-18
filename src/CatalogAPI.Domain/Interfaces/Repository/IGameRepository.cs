using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Generic;

namespace CatalogAPI.Domain.Interfaces.Repository;

public interface IGameRepository : IGenericEntityRepository<Game>
{
    Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero);
}
