namespace CatalogAPI.Domain.Dtos.Request.Game;

public record ListarGamesPaginadoRequest
{
    public int NumeroPagina { get; init; } = 1;
    public int TamanhoPagina { get; init; } = 10;


    // Filtros opcionais
    public string? Filtro { get; init; }
    public string? Genero { get; init; }
}