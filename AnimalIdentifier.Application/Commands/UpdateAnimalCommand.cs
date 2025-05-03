using MediatR;

namespace AnimalIdentifier.Application.Commands;

public class UpdateAnimalCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
}
