namespace AnimalIdentifier.Domain.Tests.Animals.Builders;

public class AnimalBuilder
{
    private string _name = "DefaultName";

    public AnimalBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public Animal Build()
    {
        return new Animal(_name);
    }
}
