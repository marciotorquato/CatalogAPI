using CatalogAPI.Domain.Events;
using CatalogAPI.Domain.Interfaces.Events;
using CatalogAPI.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogAPI.IoC;

public static class MassTransitExtensions
{
    public static IServiceCollection AddRabbitMQMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar o inicializador
        services.AddSingleton<RabbitMQInitializer>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "admin");
                    h.Password(configuration["RabbitMQ:Password"] ?? "admin");
                });

                // Apenas configurar a exchange para publicação
                cfg.Message<OrderPlacedEvent>(e =>
                {
                    e.SetEntityName(configuration["RabbitMQ:Exchanges:OrderPlaced"] ?? "order-placed-exchange");
                });

                cfg.Publish<OrderPlacedEvent>(e =>
                {
                    e.ExchangeType = "fanout";
                    e.Durable = true;
                });

                // NÃO criar ReceiveEndpoint!
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventPublisher, RabbitMQEventPublisher>();

        return services;
    }
}