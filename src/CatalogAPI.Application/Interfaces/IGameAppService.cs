using CatalogAPI.Domain.Dtos.Request.Game;
using CatalogAPI.Domain.Dtos.Responses.Game;
using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Application.Interfaces;

public interface IGameAppService
{
    Game BuscarPorId(Guid id);
    Task<Game> Cadastrar(CadastrarGameRequest request);
    Task<ListarGamesPaginadoResponse> ListarGamesPaginado(ListarGamesPaginadoRequest request);
    Task<List<GameResponse>> ListarPaginacao(int take, int skip);
    Task<(AtualizarGameResponse? Game, bool Success)> AtualizarGame(AtualizarGameRequest request);
}