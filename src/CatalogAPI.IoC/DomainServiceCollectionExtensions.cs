using CatalogAPI.Domain.Interfaces.Services;
using CatalogAPI.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CatalogAPI.IoC;

[ExcludeFromCodeCoverage]
public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GamesServices>();
        services.AddScoped<IUsuarioGameBibliotecaService, UsuarioGameBibliotecaServices>();
        services.AddScoped<IGameRatingService, GameRatingService>();
        services.AddScoped<ISearchService, SearchService>();
        return services;
    }
}