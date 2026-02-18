using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Configurations;

public class UsuarioGameBibliotecaConfiguration : IEntityTypeConfiguration<UsuarioGameBiblioteca>
{

    public void Configure(EntityTypeBuilder<UsuarioGameBiblioteca> builder)
    {
        builder.ToTable("UsuarioGameBiblioteca");
        builder.HasKey(primaryKey => primaryKey.Id);

        builder.Property(ugb => ugb.Id)
              .ValueGeneratedNever();

        builder.Property(ugb => ugb.TipoAquisicao)
               .IsRequired(false)
               .HasColumnType("nvarchar(max)");

        builder.Property(ugb => ugb.PrecoAquisicao)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(ugb => ugb.DataAquisicao)
               .IsRequired();

        builder.Property(c => c.UsuarioId)
               .IsRequired();

        // Game (1) -> Biblioteca (N)
        builder.HasOne(ugb => ugb.Game)
               .WithMany(g => g.Biblioteca)
               .HasForeignKey(ugb => ugb.GameId)
               .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(ugb => new { ugb.UsuarioId, ugb.GameId }).IsUnique();
    }
}
