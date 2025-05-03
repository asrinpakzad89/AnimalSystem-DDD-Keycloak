using AnimalIdentifier.Application.Animals.ViewModels;
using MediatR;

namespace AnimalIdentifier.Application.Queries;

public class GetAnimalByIdQuery : IRequest<AnimalDto>
{
    public int Id { get; set; }
}