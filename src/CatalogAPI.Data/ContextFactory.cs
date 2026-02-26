using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogAPI.Data;

internal class ContextFactory : IDesignTimeDbContextFactory<CatalogApiDbContext>
{
    public CatalogApiDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogApiDbContext>();
        optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=MS_CatalogAPI;Integrated security=true");

        return new CatalogApiDbContext(optionsBuilder.Options);
    }
}
