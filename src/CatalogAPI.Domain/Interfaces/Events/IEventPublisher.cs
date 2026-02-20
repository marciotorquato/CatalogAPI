using CatalogAPI.Domain.Events;

namespace CatalogAPI.Domain.Interfaces.Events;

public interface IEventPublisher
{
    Task PublishOrderPlacedAsync(OrderPlacedEvent orderEvent);
}
