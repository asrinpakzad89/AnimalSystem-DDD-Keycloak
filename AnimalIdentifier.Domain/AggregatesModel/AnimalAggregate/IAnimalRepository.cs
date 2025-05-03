
using AnimalIdentifier.Domain.Seedwork;

namespace AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;

public interface IAnimalRepository : IRepository<Animal>
{
    Animal Add(Animal animal);
    void Update(Animal animal);
    void Delete(Animal animal);
    Task<Animal> GetAsync(int animalId, CancellationToken cancellationToken);
    Task<List<Animal>> GetAllAsync(CancellationToken cancellationToken);
}