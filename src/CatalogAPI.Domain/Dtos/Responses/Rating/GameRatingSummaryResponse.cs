namespace CatalogAPI.Domain.Dtos.Responses.Rating;

public class GameRatingSummaryResponse
{
    public Guid GameId { get; set; }
    public double MediaNota { get; set; }
    public int TotalAvaliacoes { get; set; }
    public List<GameRatingResponse> Avaliacoes { get; set; } = [];
}
