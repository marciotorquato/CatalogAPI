namespace CatalogAPI.Domain.Dtos.Request.Game;

public record BuscarGameRequest
{
    public string Termo { get; init; }

    // int (não anulável) é tratado como obrigatório pelo binding do
    // Minimal API via [AsParameters], mesmo com valor padrão no C# — por
    // isso precisa ser int? aqui, com o fallback aplicado no AppService.
    public int? Take { get; init; }
}
