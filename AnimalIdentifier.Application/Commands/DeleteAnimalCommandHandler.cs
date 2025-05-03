using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AnimalIdentifier.Application.Commands;

public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand>
{
    private readonly IAnimalRepository _repository;

    public DeleteAnimalCommandHandler(IAnimalRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {

        var animal = await _repository.GetAsync(command.Id, cancellationToken);
        if (animal == null)
            throw new ValidationException("لطفا شناسه را درست وارد نمایید.");

        _repository.Delete(animal);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
