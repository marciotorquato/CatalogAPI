using CatalogAPI.Domain.Consumers;
using CatalogAPI.Domain.Interfaces.Events;
using CatalogAPI.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogAPI.IoC;

public static class RabbitMQExtensions
{
    public static IServiceCollection AddRabbitMQMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar o inicializador
        services.AddSingleton<RabbitMQInitializer>();

        // Registrar o publisher como Singleton (mantém a conexão aberta)
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        // ✅ Registrar o Consumer como Scoped
        services.AddScoped<PaymentProcessedConsumer>();

        // ✅ Registrar Background Service
        services.AddHostedService<RabbitMQConsumer>();

        return services;
    }
}