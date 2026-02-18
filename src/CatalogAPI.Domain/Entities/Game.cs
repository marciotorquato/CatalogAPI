namespace CatalogAPI.Domain.Entities;

public class Game
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Genero { get; set; }
    public string Desenvolvedor { get; set; }
    public decimal Preco { get; set; }
    public DateTimeOffset? DataCriacao { get; set; }
    public DateTimeOffset? DataRelease { get; set; }

    public virtual ICollection<UsuarioGameBiblioteca> Biblioteca { get; set; }


    public static Game Criar(string nome, string descricao, string genero, string desenvolvedor, decimal preco, DateTimeOffset dataRelease)
    {
        return new Game
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            Genero = genero,
            Desenvolvedor = desenvolvedor,
            Preco = preco,
            DataCriacao = DateTime.UtcNow,
            DataRelease = dataRelease,
        };
    }
}
