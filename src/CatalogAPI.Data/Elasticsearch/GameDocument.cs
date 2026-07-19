namespace CatalogAPI.Data.Elasticsearch;

// Formato indexado no Elasticsearch — separado da entidade Game (EF Core
// com lazy-loading proxies) para não arriscar serializar a coleção de
// navegação Biblioteca nem disparar lazy-load durante a indexação.
public class GameDocument
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Genero { get; set; }
    public string Desenvolvedor { get; set; }
    public decimal Preco { get; set; }
    public DateTimeOffset? DataRelease { get; set; }
}
