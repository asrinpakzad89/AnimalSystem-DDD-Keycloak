using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalIdentifier.Infrastructure.Data.EntityConfigurations;

public class AnimalEntityTypeConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable("animals");

        builder.Ignore(b => b.DomainEvents);

        builder.Property(a => a.Id)
             .UseHiLo("animalseq");

        builder.Property(x => x.Name)
            .HasMaxLength(200);

        builder.HasIndex(x => x.Name)
            .IsUnique(true);
    }
}
