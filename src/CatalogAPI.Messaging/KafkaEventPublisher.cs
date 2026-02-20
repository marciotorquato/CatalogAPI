using CatalogAPI.Domain.Events;
using CatalogAPI.Domain.Interfaces.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Messaging;

public class KafkaEventPublisher : IEventPublisher
{
    private readonly ITopicProducer<OrderPlacedEvent> _producer;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(
        ITopicProducer<OrderPlacedEvent> producer,
        ILogger<KafkaEventPublisher> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task PublishOrderPlacedAsync(OrderPlacedEvent orderEvent)
    {
        try
        {
            await _producer.Produce(orderEvent);
            _logger.LogInformation(
                "Evento OrderPlaced publicado com sucesso | UsuarioId: {UsuarioId} | GameId: {GameId} | Valor: {Valor}",
                orderEvent.UsuarioId,
                orderEvent.GameId,
                orderEvent.PrecoAquisicao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao publicar evento OrderPlaced | UsuarioId: {UsuarioId} | GameId: {GameId}",
                orderEvent.UsuarioId,
                orderEvent.GameId);
            throw;
        }
    }
}
