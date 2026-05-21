namespace CatalogAPI.Domain.Entities;

public class GameRating
{
    public string Id { get; set; } = null!;
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime CriadoEm { get; set; }

    public static GameRating Criar(Guid gameId, Guid userId, int nota, string? comentario)
        => new()
        {
            GameId = gameId,
            UserId = userId,
            Nota = nota,
            Comentario = comentario,
            CriadoEm = DateTime.UtcNow
        };
}
