namespace AnimalIdentifier.Application.Animals.ViewModels;

public record AnimalDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record CreateAnimalDto
{
    public string Name { get; set; }
}
public record DeleteAnimalDto
{
    public int Id { get; set; }
}
public record DisplayAnimalDto
{
    public int Id { get; set; }
}
