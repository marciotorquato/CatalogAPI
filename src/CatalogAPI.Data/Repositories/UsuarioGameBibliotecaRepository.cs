using CatalogAPI.Data.Repositories.Generic;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data.Repositories;

public class UsuarioGameBibliotecaRepository : GenericEntityRepository<UsuarioGameBiblioteca>, IUsuarioGameBibliotecaRepository
{
    public UsuarioGameBibliotecaRepository(CatalogApiDbContext context) : base(context)
    {
    }

    public List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId)
    {
        return _dbSet
            .AsNoTracking()
            .Include(b => b.Game)
            .Where(b => b.UsuarioId == usuarioId)
            .ToList();
    }

    public UsuarioGameBiblioteca? BuscarPorIdEUsuario(Guid id, Guid usuarioId)
    {
        return _dbSet
            .Include(b => b.Game)
            .FirstOrDefault(b => b.Id == id && b.UsuarioId == usuarioId);
    }

    public bool UsuarioJaPossuiGame(Guid usuarioId, Guid gameId)
    {
        return _dbSet.Any(b => b.UsuarioId == usuarioId && b.GameId == gameId);
    }
}