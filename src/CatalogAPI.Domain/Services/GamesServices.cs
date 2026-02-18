using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Generic;
using CatalogAPI.Domain.Interfaces.Repository;
using CatalogAPI.Domain.Interfaces.Services;
using CatalogAPI.Domain.Services.Generic;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Domain.Services;

public class GamesServices : GenericServices<Game>, IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GamesServices> _logger;

    public GamesServices(
        IGenericEntityRepository<Game> repository,
        IGameRepository gameRepository,
        ILogger<GamesServices> logger) : base(repository)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero)
    {
        return await _gameRepository.ListarPaginado(numeroPagina, tamanhoPagina, filtro, genero);
    }

    public async Task<(Game? Game, bool Success)> AtualizarGame(Game game)
    {
        var gameExistente = _repository.GetById(game.Id);
        if (gameExistente == null)
        {
            _logger.LogWarning("Jogo não encontrado para atualização | GameId: {GameId}", game.Id);
            return (null, false);
        }
        gameExistente.Nome = game.Nome;
        gameExistente.Descricao = game.Descricao;
        gameExistente.Genero = game.Genero;
        gameExistente.Desenvolvedor = game.Desenvolvedor;
        gameExistente.Preco = game.Preco;
        gameExistente.DataRelease = game.DataRelease;
        var resultado = _repository.Update(gameExistente);
        if (!resultado.success)
        {
            _logger.LogError("Falha ao atualizar jogo no repositório | GameId: {GameId}", game.Id);
        }
        return (resultado.entity, resultado.success);
    }
}