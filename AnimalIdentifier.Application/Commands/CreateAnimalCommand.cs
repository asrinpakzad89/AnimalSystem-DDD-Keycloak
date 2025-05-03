using MediatR;

namespace AnimalIdentifier.Application.Commands;

public class CreateAnimalCommand : IRequest
{
    public string Name { get; set; }
}
