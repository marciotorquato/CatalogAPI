namespace CatalogAPI.Domain.Dtos.Responses.Search;

public class GameSearchResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Genero { get; set; }
    public string Desenvolvedor { get; set; }
    public decimal Preco { get; set; }
    public DateTimeOffset? DataRelease { get; set; }
}
