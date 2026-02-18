using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Generic;

namespace CatalogAPI.Domain.Interfaces.Services;

public interface IUsuarioGameBibliotecaService : IGenericServices<UsuarioGameBiblioteca>
{
    List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId);
    Task<(UsuarioGameBiblioteca? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(UsuarioGameBiblioteca biblioteca);
    Task<(UsuarioGameBiblioteca? Biblioteca, bool Success)> Atualizar(UsuarioGameBiblioteca biblioteca);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}
