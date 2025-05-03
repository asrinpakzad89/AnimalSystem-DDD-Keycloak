using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Seedwork;
using AnimalIdentifier.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace AnimalIdentifier.Infrastructure.Data;

public class AnimalDbContext : DbContext, IUnitOfWork
{
    public AnimalDbContext(DbContextOptions<AnimalDbContext> options)
    : base(options)
    {
    }
    public DbSet<Animal> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnimalEntityTypeConfiguration());
        modelBuilder.HasSequence<int>("animalseq")
        .StartsAt(1)
        .IncrementsBy(1);
        
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return true;
    }
}

