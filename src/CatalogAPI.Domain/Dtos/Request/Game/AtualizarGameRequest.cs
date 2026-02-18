namespace CatalogAPI.Domain.Dtos.Request.Game;

public record AtualizarGameRequest
{
    public Guid Id { get; init; }
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public string Genero { get; init; }
    public string Desenvolvedor { get; init; }
    public decimal Preco { get; init; }
    public DateOnly? DataRelease { get; init; }
}