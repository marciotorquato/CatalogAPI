using CatalogAPI.Domain.Events;
using CatalogAPI.Domain.Interfaces.Events;
using CatalogAPI.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogAPI.IoC;

public static class MassTransitExtensions
{
    public static IServiceCollection AddKafkaMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

            x.AddRider(rider =>
            {
                rider.AddProducer<OrderPlacedEvent>("order-placed-topic");

                rider.UsingKafka((context, k) =>
                {
                    k.Host(configuration["Kafka:BootstrapServers"] ?? "localhost:9092");
                });
            });
        });

        services.AddScoped<IEventPublisher, KafkaEventPublisher>();

        return services;
    }
}