using CatalogAPI.Domain.Enum;

namespace CatalogAPI.Domain.Entities;

public class UsuarioGameBiblioteca
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid GameId { get; set; }
    public string TipoAquisicao { get; set; }
    public decimal PrecoAquisicao { get; set; }
    public DateTimeOffset? DataAquisicao { get; set; }
    public string Status { get; set; } = StatusCompra.EmProcessamento;
    public DateTimeOffset? DataAtualizacaoStatus { get; set; }

    public virtual Game Game { get; set; }
}
