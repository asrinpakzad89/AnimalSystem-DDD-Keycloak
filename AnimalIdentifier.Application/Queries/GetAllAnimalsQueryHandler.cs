using AnimalIdentifier.Application.Animals.ViewModels;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AutoMapper;
using MediatR;

namespace AnimalIdentifier.Application.Queries;

public class GetAllAnimalsQueryHandler : IRequestHandler<GetAllAnimalsQuery, List<AnimalDto>>
{
    private readonly IAnimalRepository _repository;
    private readonly IMapper _mapper;

    public GetAllAnimalsQueryHandler(IAnimalRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<AnimalDto>> Handle(GetAllAnimalsQuery query, CancellationToken cancellationToken)
    {
        var animals = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<AnimalDto>>(animals);
    }
}
