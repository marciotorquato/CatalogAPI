using CatalogAPI.Data.Configurations;
using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data;

public class CatalogApiDbContext : DbContext
{
    public CatalogApiDbContext(DbContextOptions<CatalogApiDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogApiDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioGameBibliotecaConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Game> Game { set; get; }
    public DbSet<UsuarioGameBiblioteca> UsuarioGameBiblioteca { set; get; }
}
