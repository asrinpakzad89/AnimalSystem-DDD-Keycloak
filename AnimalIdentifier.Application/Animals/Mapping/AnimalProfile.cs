using AnimalIdentifier.Application.Animals.ViewModels;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AutoMapper;

namespace AnimalIdentifier.Application.Animals.Mapping;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<Animal, AnimalDto>().ReverseMap();
    }
}
