using AnimalIdentifier.Domain.Events;
using MediatR;

namespace AnimalIdentifier.Application.DomainEventHandlers;

public class AnimalCreatedDomainEventHandler : INotificationHandler<AnimalCreatedDomainEvent>
{
    public Task Handle(AnimalCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"A new animal created: {notification.AnimalName}");
        return Task.CompletedTask;
    }
}
