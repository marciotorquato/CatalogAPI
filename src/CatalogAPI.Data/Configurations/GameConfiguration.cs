using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Data.Configurations;

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Game");
        builder.HasKey(primaryKey => primaryKey.Id);

        builder.Property(g => g.Id)
              .ValueGeneratedNever();

        builder.Property(g => g.Nome)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Descricao)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Genero)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.Desenvolvedor)
               .IsRequired(true)
               .HasColumnType("nvarchar(max)");

        builder.Property(g => g.DataRelease)
               .IsRequired();

        builder.Property(g => g.Preco)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(g => g.DataCriacao)
               .IsRequired(false);

        builder.HasMany(g => g.Biblioteca)
              .WithOne(b => b.Game)
              .HasForeignKey(b => b.GameId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}