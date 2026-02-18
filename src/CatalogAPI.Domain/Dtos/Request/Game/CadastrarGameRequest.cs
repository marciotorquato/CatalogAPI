namespace CatalogAPI.Domain.Dtos.Request.Game;
public class CadastrarGameRequest
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Genero { get; set; }
    public string Desenvolvedor { get; set; }
    public DateTime DataRelease { get; set; }
    public decimal Preco { get; set; }
}