using AnimalIdentifier.Application.Animals.ViewModels;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AutoMapper;
using MediatR;

namespace AnimalIdentifier.Application.Queries;

public class GetAnimalByIdQueryHandler : IRequestHandler<GetAnimalByIdQuery, AnimalDto>
{
    private readonly IAnimalRepository _repository;
    private readonly IMapper _mapper;
    public GetAnimalByIdQueryHandler(IAnimalRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AnimalDto> Handle(GetAnimalByIdQuery query, CancellationToken cancellationToken)
    {
        var animal = await _repository.GetAsync(query.Id, cancellationToken);
        if (animal == null)
            throw new Exception("لطفا شناسه را درست وارد نمایید.");

        return _mapper.Map<AnimalDto>(animal);
    }
}
