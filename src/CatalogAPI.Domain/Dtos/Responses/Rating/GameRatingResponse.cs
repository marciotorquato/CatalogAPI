namespace CatalogAPI.Domain.Dtos.Responses.Rating;

public class GameRatingResponse
{
    public string Id { get; set; } = null!;
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime CriadoEm { get; set; }
}
