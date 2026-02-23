using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CatalogAPI.Messaging;

public class RabbitMQInitializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQInitializer> _logger;

    public RabbitMQInitializer(IConfiguration configuration, ILogger<RabbitMQInitializer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = _configuration["RabbitMQ:Username"] ?? "admin",
            Password = _configuration["RabbitMQ:Password"] ?? "admin",
            VirtualHost = "/",
            Port = 5672
        };

        try
        {
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // ===== CONFIGURAÇÃO PARA PUBLICAR (OrderPlaced) =====
            var orderPlacedExchange = _configuration["RabbitMQ:Exchanges:OrderPlaced"] ?? "order-placed-exchange";
            await channel.ExchangeDeclareAsync(
                exchange: orderPlacedExchange,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            var orderPlacedQueue = "order-placed-queue";
            await channel.QueueDeclareAsync(
                queue: orderPlacedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: orderPlacedQueue,
                exchange: orderPlacedExchange,
                routingKey: "",
                arguments: null
            );

            _logger.LogInformation(
                "RabbitMQ OrderPlaced configurado | Exchange: {Exchange} | Queue: {Queue}",
                orderPlacedExchange,
                orderPlacedQueue);

            // ===== CONFIGURAÇÃO PARA CONSUMIR (PaymentProcessed) =====
            var paymentProcessedExchange = _configuration["RabbitMQ:Exchanges:PaymentProcessed"] ?? "payment-processed-exchange";
            await channel.ExchangeDeclareAsync(
                exchange: paymentProcessedExchange,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            var paymentProcessedQueue = "payment-processed-queue";
            await channel.QueueDeclareAsync(
                queue: paymentProcessedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: paymentProcessedQueue,
                exchange: paymentProcessedExchange,
                routingKey: "",
                arguments: null
            );

            _logger.LogInformation(
                "RabbitMQ PaymentProcessed configurado | Exchange: {Exchange} | Queue: {Queue}",
                paymentProcessedExchange,
                paymentProcessedQueue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inicializar RabbitMQ");
            throw;
        }
    }
}