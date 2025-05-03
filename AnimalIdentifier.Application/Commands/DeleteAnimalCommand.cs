using MediatR;

namespace AnimalIdentifier.Application.Commands;

public class DeleteAnimalCommand :IRequest
{
    public int Id { get; set; }
}
