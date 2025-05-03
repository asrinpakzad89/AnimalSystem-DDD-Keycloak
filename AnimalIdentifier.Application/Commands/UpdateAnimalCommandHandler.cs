using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using FluentValidation;
using MediatR;

namespace AnimalIdentifier.Application.Commands;

public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, Unit>
{
    private readonly IAnimalRepository _repository;
    private readonly IValidator<UpdateAnimalCommand> _validator;

    public UpdateAnimalCommandHandler(IAnimalRepository repository, IValidator<UpdateAnimalCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Unit> Handle(UpdateAnimalCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var animal = await _repository.GetAsync(command.Id, cancellationToken);
        if (animal == null)
            throw new ValidationException("لطفا شناسه را درست وارد نمایید.");

        animal.UpdateName(command.Name);
        _repository.Update(animal);
        await _repository.UnitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}

