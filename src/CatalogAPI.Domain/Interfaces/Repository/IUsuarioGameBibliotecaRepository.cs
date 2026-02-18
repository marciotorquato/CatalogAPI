using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Generic;

namespace CatalogAPI.Domain.Interfaces.Repository;

public interface IUsuarioGameBibliotecaRepository : IGenericEntityRepository<UsuarioGameBiblioteca>
{
    List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId);
    UsuarioGameBiblioteca? BuscarPorIdEUsuario(Guid id, Guid usuarioId);
    bool UsuarioJaPossuiGame(Guid usuarioId, Guid gameId);
}