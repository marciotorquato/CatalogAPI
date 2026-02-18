using CatalogAPI.Data.Repositories;
using CatalogAPI.Data.Repositories.Generic;
using CatalogAPI.Domain.Interfaces.Generic;
using CatalogAPI.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CatalogAPI.IoC;

[ExcludeFromCodeCoverage]
public static class RepositoryCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericEntityRepository<>), typeof(GenericEntityRepository<>));
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IUsuarioGameBibliotecaRepository, UsuarioGameBibliotecaRepository>();
        return services;
    }
}