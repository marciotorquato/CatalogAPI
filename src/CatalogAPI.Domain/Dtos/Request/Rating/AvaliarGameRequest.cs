namespace CatalogAPI.Domain.Dtos.Request.Rating;

public class AvaliarGameRequest
{
    public Guid UsuarioId { get; set; }

    /// <summary>Nota de 1 a 5.</summary>
    public int Nota { get; set; }

    public string? Comentario { get; set; }
}
