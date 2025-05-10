using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using FluentValidation;
using MediatR;

namespace AnimalIdentifier.Application.Commands;

public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand>
{
    private readonly IAnimalRepository _repository;
    private readonly IValidator<DeleteAnimalCommand> _validator;

    public DeleteAnimalCommandHandler(IAnimalRepository repository, IValidator<DeleteAnimalCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Unit> Handle(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var animal = await _repository.GetAsync(command.Id, cancellationToken);
        if (animal == null)
            throw new ValidationException("لطفا شناسه را درست وارد نمایید.");

        _repository.Delete(animal);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
