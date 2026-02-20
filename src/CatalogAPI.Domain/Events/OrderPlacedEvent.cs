namespace CatalogAPI.Domain.Events;

public record OrderPlacedEvent
{
    public Guid UsuarioId { get; init; }
    public Guid GameId { get; init; }
    public decimal PrecoAquisicao { get; init; }
    public DateTimeOffset DataCompra { get; init; }
}