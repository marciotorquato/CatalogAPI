using CatalogAPI.Data.Repositories.Generic;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data.Repositories;

public class GameRepository : GenericEntityRepository<Game>, IGameRepository
{
    public GameRepository(CatalogApiDbContext context) : base(context)
    {
    }

    public async Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero)
    {
        var query = _dbSet.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            query = query.Where(g => g.Nome.Contains(filtro) ||
                                     g.Descricao.Contains(filtro));
        }

        if (!string.IsNullOrWhiteSpace(genero))
        {
            query = query.Where(g => g.Genero == genero);
        }

        // Total de registros
        var totalRegistros = await query.CountAsync();

        // Ordenação e paginação
        var jogos = await query
            .OrderByDescending(g => g.DataRelease)
            .Skip((numeroPagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (jogos, totalRegistros);
    }
}