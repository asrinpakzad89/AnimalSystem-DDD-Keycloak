using AnimalIdentifier.Domain.Seedwork;
using AnimalIdentifier.Domain.SeedWork;

namespace AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;

public class Animal : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    protected Animal() { }

    public Animal(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Animal name cannot be empty.", nameof(name));

        Name = name;
    }

    public void UpdateName(string animalName)
    {
        if (string.IsNullOrWhiteSpace(animalName))
            throw new ArgumentException("Animal name cannot be empty.", nameof(animalName));

        Name = animalName;
    }
}