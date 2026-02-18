using CatalogAPI.Application.Interfaces;
using CatalogAPI.Domain.Dtos.Request.UsuarioGameBiblioteca;
using CatalogAPI.Domain.Dtos.Responses.UsuarioGameBiblioteca;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Exceptions;
using CatalogAPI.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Application.AppServices;

public class UsuarioGameBibliotecaAppService : IUsuarioGameBibliotecaAppService
{
    private readonly IUsuarioGameBibliotecaService _bibliotecaService;
    private readonly IGameService _gameService;
    private readonly ILogger<UsuarioGameBibliotecaAppService> _logger;

    public UsuarioGameBibliotecaAppService(
        IUsuarioGameBibliotecaService bibliotecaService,
        IGameService gameService,
        ILogger<UsuarioGameBibliotecaAppService> logger)
    {
        _bibliotecaService = bibliotecaService;
        _gameService = gameService;
        _logger = logger;
    }

    public async Task<List<BibliotecaResponse>> ListarPorUsuario(Guid usuarioId)
    {
        // Adicionar requisição api UsersApi buscando pelo UsuarioId para verificar se ele realmente existe
        //var usuarioExiste = _usuarioService.GetById(usuarioId);
        //if (usuarioExiste == null)
        //{
        //    _logger.LogWarning("Usuário não encontrado ao listar biblioteca | UsuarioId: {UsuarioId}", usuarioId);
        //    throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");
        //}

        var biblioteca = _bibliotecaService.ListarPorUsuario(usuarioId);
        var bibliotecaResponse = biblioteca.Select(b => new BibliotecaResponse
        {
            Id = b.Id,
            UsuarioId = b.UsuarioId,
            GameId = b.GameId,
            TipoAquisicao = b.TipoAquisicao,
            PrecoAquisicao = b.PrecoAquisicao,
            DataAquisicao = b.DataAquisicao,
            NomeGame = b.Game?.Nome,
            GeneroGame = b.Game?.Genero
        }).ToList();
        return await Task.FromResult(bibliotecaResponse);
    }

    public async Task<(BibliotecaResponse? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(ComprarGameRequest request)
    {
        //var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        //if (usuarioExiste == null)
        //{
        //    _logger.LogWarning("Usuário não encontrado para comprar jogo | UsuarioId: {UsuarioId} | GameId: {GameId}", request.UsuarioId, request.GameId);
        //    throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");
        //}

        var gameExiste = _gameService.GetById(request.GameId);
        if (gameExiste == null)
        {
            _logger.LogWarning("Jogo não encontrado para compra | GameId: {GameId} | UsuarioId: {UsuarioId}", request.GameId, request.UsuarioId);
            throw new NotFoundException($"Jogo com ID {request.GameId} não encontrado.");
        }

        var biblioteca = new UsuarioGameBiblioteca
        {
            UsuarioId = request.UsuarioId,
            GameId = request.GameId,
            TipoAquisicao = request.TipoAquisicao,
            PrecoAquisicao = request.PrecoAquisicao,
            DataAquisicao = request.DataAquisicao
        };
        var (bibliotecaComprada, sucesso, errorMessage) = await _bibliotecaService.ComprarGame(biblioteca);
        if (!sucesso || bibliotecaComprada == null)
        {
            _logger.LogWarning("Falha na compra do jogo | UsuarioId: {UsuarioId} | GameId: {GameId} | Erro: {ErrorMessage}", request.UsuarioId, request.GameId, errorMessage);
            return (null, false, errorMessage);
        }
        var response = new BibliotecaResponse
        {
            Id = bibliotecaComprada.Id,
            UsuarioId = bibliotecaComprada.UsuarioId,
            GameId = bibliotecaComprada.GameId,
            TipoAquisicao = bibliotecaComprada.TipoAquisicao,
            PrecoAquisicao = bibliotecaComprada.PrecoAquisicao,
            DataAquisicao = bibliotecaComprada.DataAquisicao,
            NomeGame = bibliotecaComprada.Game?.Nome,
            GeneroGame = bibliotecaComprada.Game?.Genero
        };
        return (response, true, null);
    }

    public async Task<(BibliotecaResponse? Biblioteca, bool Success)> Atualizar(AtualizarBibliotecaRequest request)
    {
        //var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        //if (usuarioExiste == null)
        //{
        //    _logger.LogWarning("Usuário não encontrado para atualizar item da biblioteca | UsuarioId: {UsuarioId} | BibliotecaItemId: {BibliotecaItemId}", request.UsuarioId, request.Id);
        //    throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");
        //}

        var gameExiste = _gameService.GetById(request.GameId);
        if (gameExiste == null)
        {
            _logger.LogWarning("Jogo não encontrado para atualização de item da biblioteca | GameId: {GameId} | UsuarioId: {UsuarioId} | BibliotecaItemId: {BibliotecaItemId}", request.GameId, request.UsuarioId, request.Id);
            throw new NotFoundException($"Jogo com ID {request.GameId} não encontrado.");
        }

        var biblioteca = new UsuarioGameBiblioteca
        {
            Id = request.Id,
            UsuarioId = request.UsuarioId,
            GameId = request.GameId,
            TipoAquisicao = request.TipoAquisicao,
            PrecoAquisicao = request.PrecoAquisicao,
            DataAquisicao = request.DataAquisicao
        };
        var (bibliotecaAtualizada, sucesso) = await _bibliotecaService.Atualizar(biblioteca);
        if (!sucesso || bibliotecaAtualizada == null)
        {
            _logger.LogWarning("Falha ao atualizar item da biblioteca ou item não encontrado | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", request.Id, request.UsuarioId);
            return (null, false);
        }
        var response = new BibliotecaResponse
        {
            Id = bibliotecaAtualizada.Id,
            UsuarioId = bibliotecaAtualizada.UsuarioId,
            GameId = bibliotecaAtualizada.GameId,
            TipoAquisicao = bibliotecaAtualizada.TipoAquisicao,
            PrecoAquisicao = bibliotecaAtualizada.PrecoAquisicao,
            DataAquisicao = bibliotecaAtualizada.DataAquisicao,
            NomeGame = bibliotecaAtualizada.Game?.Nome,
            GeneroGame = bibliotecaAtualizada.Game?.Genero
        };
        return (response, true);
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        //var usuarioExiste = _usuarioService.GetById(usuarioId);
        //if (usuarioExiste == null)
        //{
        //    _logger.LogWarning("Usuário não encontrado para deletar item da biblioteca | UsuarioId: {UsuarioId} | BibliotecaItemId: {BibliotecaItemId}", usuarioId, id);
        //    throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");
        //}

        return await _bibliotecaService.Deletar(id, usuarioId);
    }
}