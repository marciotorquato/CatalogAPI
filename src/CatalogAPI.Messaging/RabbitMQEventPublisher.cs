using CatalogAPI.Domain.Events;
using CatalogAPI.Domain.Interfaces.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CatalogAPI.Messaging;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RabbitMQEventPublisher> _logger;

    public RabbitMQEventPublisher(IPublishEndpoint publishEndpoint, ILogger<RabbitMQEventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishOrderPlacedAsync(OrderPlacedEvent orderEvent)
    {
        try
        {
            await _publishEndpoint.Publish(orderEvent);

            _logger.LogInformation(
                "Evento OrderPlaced publicado com sucesso no RabbitMQ | UsuarioId: {UsuarioId} | GameId: {GameId} | Valor: {Valor}",
                orderEvent.UsuarioId,
                orderEvent.GameId,
                orderEvent.PrecoAquisicao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao publicar evento OrderPlaced no RabbitMQ | UsuarioId: {UsuarioId} | GameId: {GameId}",
                orderEvent.UsuarioId,
                orderEvent.GameId);
            throw;
        }
    }
}