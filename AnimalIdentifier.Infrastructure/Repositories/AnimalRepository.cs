using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Seedwork;
using AnimalIdentifier.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimalIdentifier.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly AnimalDbContext _context;

    public AnimalRepository(AnimalDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public Animal Add(Animal animal)
    {
        return _context.Animals.Add(animal).Entity;
    }

    public void Update(Animal animal)
    {
        _context.Entry(animal).State = EntityState.Modified;
    }
    public void Delete(Animal animal)
    {
        _context.Animals.Remove(animal);
    }

    public async Task<Animal> GetAsync(int animalId, CancellationToken cancellationToken)
    {
        return await _context.Animals.FindAsync(animalId, cancellationToken);
    }

    public async Task<List<Animal>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Animals.AsNoTracking().ToListAsync();
    }
}

