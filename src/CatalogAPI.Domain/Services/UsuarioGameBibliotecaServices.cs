using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Generic;
using CatalogAPI.Domain.Interfaces.Repository;
using CatalogAPI.Domain.Interfaces.Services;
using CatalogAPI.Domain.Services.Generic;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Domain.Services;

public class UsuarioGameBibliotecaServices : GenericServices<UsuarioGameBiblioteca>, IUsuarioGameBibliotecaService
{
    private readonly IUsuarioGameBibliotecaRepository _bibliotecaRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<UsuarioGameBibliotecaServices> _logger;

    public UsuarioGameBibliotecaServices(
        IGenericEntityRepository<UsuarioGameBiblioteca> repository,
        IUsuarioGameBibliotecaRepository bibliotecaRepository,
        IGameRepository gameRepository,
        ILogger<UsuarioGameBibliotecaServices> logger) : base(repository)
    {
        _bibliotecaRepository = bibliotecaRepository;
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId)
    {
        return _bibliotecaRepository.ListarPorUsuario(usuarioId);
    }

    public async Task<(UsuarioGameBiblioteca? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(UsuarioGameBiblioteca biblioteca)
    {
        var game = _gameRepository.GetById(biblioteca.GameId);
        if (game == null)
        {
            _logger.LogWarning("Game não encontrado ao tentar comprar | GameId: {GameId} | UsuarioId: {UsuarioId}", biblioteca.GameId, biblioteca.UsuarioId);
            return (null, false, "Game não encontrado.");
        }
        if (_bibliotecaRepository.UsuarioJaPossuiGame(biblioteca.UsuarioId, biblioteca.GameId))
        {
            _logger.LogWarning("Usuário já possui o jogo na biblioteca | GameId: {GameId} | UsuarioId: {UsuarioId}", biblioteca.GameId, biblioteca.UsuarioId);
            return (null, false, "Você já possui este jogo na sua biblioteca.");
        }
        biblioteca.Id = Guid.NewGuid();
        biblioteca.DataAquisicao ??= DateTimeOffset.UtcNow;
        var resultado = await _repository.Insert(biblioteca);
        if (resultado == null)
        {
            _logger.LogError("Falha ao inserir item na biblioteca do usuário | UsuarioId: {UsuarioId} | GameId: {GameId}", biblioteca.UsuarioId, biblioteca.GameId);
            return (null, false, "Não foi possível adicionar o jogo à biblioteca.");
        }
        return (resultado, true, null);
    }

    public async Task<(UsuarioGameBiblioteca? Biblioteca, bool Success)> Atualizar(UsuarioGameBiblioteca biblioteca)
    {
        var bibliotecaExistente = _bibliotecaRepository.BuscarPorIdEUsuario(biblioteca.Id, biblioteca.UsuarioId);
        if (bibliotecaExistente == null)
        {
            _logger.LogWarning("Item da biblioteca não encontrado para atualização | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", biblioteca.Id, biblioteca.UsuarioId);
            return (null, false);
        }
        bibliotecaExistente.TipoAquisicao = biblioteca.TipoAquisicao;
        bibliotecaExistente.PrecoAquisicao = biblioteca.PrecoAquisicao;
        bibliotecaExistente.DataAquisicao = biblioteca.DataAquisicao;
        var resultado = _repository.Update(bibliotecaExistente);
        if (!resultado.success)
        {
            _logger.LogError("Falha ao atualizar item da biblioteca no repositório | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", biblioteca.Id, biblioteca.UsuarioId);
        }
        return await Task.FromResult((resultado.entity, resultado.success));
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var biblioteca = _bibliotecaRepository.BuscarPorIdEUsuario(id, usuarioId);
        if (biblioteca == null)
        {
            _logger.LogWarning("Item da biblioteca não encontrado para exclusão | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", id, usuarioId);
            return false;
        }
        var sucesso = await _repository.DeleteById(id);
        if (!sucesso)
        {
            _logger.LogError("Falha ao deletar item da biblioteca no repositório | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", id, usuarioId);
        }
        return sucesso;
    }
}