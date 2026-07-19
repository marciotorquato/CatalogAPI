namespace CatalogAPI.Domain.Dtos.Request.Game;

public record BuscarGameRequest
{
    public string Termo { get; init; }
    public int Take { get; init; } = 20;
}
