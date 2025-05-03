using AnimalIdentifier.Application.Animals.ViewModels;
using MediatR;

namespace AnimalIdentifier.Application.Queries;

public class GetAllAnimalsQuery :IRequest<List<AnimalDto>>
{}