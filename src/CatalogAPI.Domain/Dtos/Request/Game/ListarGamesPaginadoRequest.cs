namespace CatalogAPI.Domain.Dtos.Request.Game;

public record ListarGamesPaginadoRequest
{
    // int (não anulável) é tratado como obrigatório pelo binding do
    // Minimal API via [AsParameters], mesmo com valor padrão no C# — por
    // isso precisam ser int? aqui, com o fallback aplicado no AppService.
    public int? NumeroPagina { get; init; }
    public int? TamanhoPagina { get; init; }


    // Filtros opcionais
    public string? Filtro { get; init; }
    public string? Genero { get; init; }
}