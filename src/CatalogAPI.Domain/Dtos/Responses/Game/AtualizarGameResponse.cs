namespace CatalogAPI.Domain.Dtos.Responses.Game;

public record AtualizarGameResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public string Genero { get; init; }
    public string Desenvolvedor { get; init; }
    public decimal Preco { get; init; }
    public DateTimeOffset? DataCriacao { get; init; }
    public DateTimeOffset? DataRelease { get; init; }
}
