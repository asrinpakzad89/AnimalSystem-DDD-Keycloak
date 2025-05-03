using MediatR;

namespace AnimalIdentifier.Domain.Events;

public class AnimalCreatedDomainEvent : INotification
{
    public int AnimalId { get; }
    public string AnimalName { get; }

    public AnimalCreatedDomainEvent(int animalId, string animalName)
    {
        AnimalId = animalId;
        AnimalName = animalName;
    }
}
