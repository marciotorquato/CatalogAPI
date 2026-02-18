namespace CatalogAPI.Domain.Dtos.Responses.Game;

public record ListarGamesPaginadoResponse
{
    public int PaginaAtual { get; init; }
    public int TamanhoPagina { get; init; }
    public int TotalPaginas { get; init; }
    public int TotalRegistros { get; init; }
    public bool TemPaginaAnterior { get; init; }
    public bool TemProximaPagina { get; init; }
    public List<GameResponse> Jogos { get; init; }
}

public record GameResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public string Genero { get; init; }
    public string Desenvolvedor { get; init; }
    public decimal Preco { get; init; }
    public DateTimeOffset? DataRelease { get; init; }
}
