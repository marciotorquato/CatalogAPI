using CatalogAPI.Application.AppServices;
using CatalogAPI.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CatalogAPI.IoC;

[ExcludeFromCodeCoverage]
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGameAppService, GameAppService>();
        services.AddScoped<IUsuarioGameBibliotecaAppService, UsuarioGameBibliotecaAppService>();
        return services;
    }
}