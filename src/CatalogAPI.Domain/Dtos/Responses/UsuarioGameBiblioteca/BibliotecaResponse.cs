namespace CatalogAPI.Domain.Dtos.Responses.UsuarioGameBiblioteca;

public record BibliotecaResponse
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid GameId { get; init; }
    public string TipoAquisicao { get; init; }
    public decimal PrecoAquisicao { get; init; }
    public DateTimeOffset? DataAquisicao { get; init; }

    // Informações do jogo (opcional, para enriquecer a resposta)
    public string? NomeGame { get; init; }
    public string? GeneroGame { get; init; }
}
