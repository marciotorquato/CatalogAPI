using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.Game;
using CatalogAPI.Domain.Dtos.Responses.Game;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Exceptions;
using CatalogAPI.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Application.AppServices;

public class GameAppService : IGameAppService
{
    private readonly IGameService _gameService;
    private readonly ISearchService _searchService;
    private readonly ILogger<GameAppService> _logger;

    public GameAppService(
        IGameService gameService,
        ISearchService searchService,
        ILogger<GameAppService> logger)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        _logger = logger;
    }

    public async Task<Game> Cadastrar(CadastrarGameRequest request)
    {
        var game = Game.Criar(request.Nome, request.Descricao, request.Genero, request.Desenvolvedor, request.Preco, request.DataRelease);
        var gameCriado = await _gameService.Insert(game);

        // Sincroniza o índice de busca sempre que um jogo é inserido no
        // banco principal (exigência da Fase 4).
        await _searchService.IndexarAsync(gameCriado);

        return gameCriado;
    }

    public Game BuscarPorId(Guid id)
    {
        var entity = _gameService.GetById(id);
        if (entity is null)
        {
            _logger.LogWarning("Game não encontrado | GameId: {GameId}", id);
            throw new NotFoundException("Game não encontrado.");
        }
        return entity;
    }

    public async Task<ListarGamesPaginadoResponse> ListarGamesPaginado(ListarGamesPaginadoRequest request)
    {
        var numeroPagina = request.NumeroPagina ?? 1;
        var tamanhoPagina = request.TamanhoPagina ?? 10;

        var (jogos, totalRegistros) = await _gameService.ListarPaginado(numeroPagina, tamanhoPagina, request.Filtro, request.Genero);
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);
        var jogosResponse = jogos.Select(g => new GameResponse
        {
            Id = g.Id,
            Nome = g.Nome,
            Descricao = g.Descricao,
            Genero = g.Genero,
            Desenvolvedor = g.Desenvolvedor,
            Preco = g.Preco,
            DataRelease = g.DataRelease
        }).ToList();
        return new ListarGamesPaginadoResponse
        {
            PaginaAtual = numeroPagina,
            TamanhoPagina = tamanhoPagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros,
            TemPaginaAnterior = numeroPagina > 1,
            TemProximaPagina = numeroPagina < totalPaginas,
            Jogos = jogosResponse
        };
    }

    public async Task<List<GameResponse>> ListarPaginacao(int take, int skip)
    {
        var games = await _gameService.ListarPaginacao(take, skip);
        var gamesResponse = games.Select(g => new GameResponse
        {
            Id = g.Id,
            Nome = g.Nome,
            Descricao = g.Descricao,
            Genero = g.Genero,
            Desenvolvedor = g.Desenvolvedor,
            Preco = g.Preco,
            DataRelease = g.DataRelease
        }).ToList();
        return await Task.FromResult(gamesResponse);
    }

    public async Task<(AtualizarGameResponse? Game, bool Success)> AtualizarGame(AtualizarGameRequest request)
    {
        var game = new Game
        {
            Id = request.Id,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Genero = request.Genero,
            Desenvolvedor = request.Desenvolvedor,
            Preco = request.Preco,
            DataRelease = request.DataRelease.HasValue
            ? new DateTimeOffset(request.DataRelease.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)
            : null
        };
        var (gameAtualizado, sucesso) = await _gameService.AtualizarGame(game);
        if (!sucesso || gameAtualizado == null)
        {
            _logger.LogWarning("Falha ao atualizar game ou game não encontrado | GameId: {GameId} | Request: {@Request}", request.Id, request);
            return (null, false);
        }

        // Sincroniza o índice de busca sempre que um jogo é editado no
        // banco principal (exigência da Fase 4).
        await _searchService.IndexarAsync(gameAtualizado);

        var response = new AtualizarGameResponse
        {
            Id = gameAtualizado.Id,
            Nome = gameAtualizado.Nome,
            Descricao = gameAtualizado.Descricao,
            Genero = gameAtualizado.Genero,
            Desenvolvedor = gameAtualizado.Desenvolvedor,
            Preco = gameAtualizado.Preco,
            DataCriacao = gameAtualizado.DataCriacao,
            DataRelease = gameAtualizado.DataRelease
        };
        return (response, true);
    }
}