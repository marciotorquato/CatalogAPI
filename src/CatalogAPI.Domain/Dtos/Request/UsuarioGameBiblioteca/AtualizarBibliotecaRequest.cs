namespace CatalogAPI.Domain.Dtos.Request.UsuarioGameBiblioteca;

public record AtualizarBibliotecaRequest
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid GameId { get; init; }
    public string TipoAquisicao { get; init; }
    public decimal PrecoAquisicao { get; init; }
    public DateTimeOffset? DataAquisicao { get; init; }
}