using FluentValidation;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using MediatR;
using AnimalIdentifier.Domain.Events;

namespace AnimalIdentifier.Application.Commands;
public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, Unit>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IValidator<CreateAnimalCommand> _validator;
    private readonly IMediator _mediator;

    public CreateAnimalCommandHandler(IAnimalRepository animalRepository, IValidator<CreateAnimalCommand> validator, IMediator mediator)
    {
        _animalRepository = animalRepository;
        _validator = validator;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(CreateAnimalCommand command, CancellationToken cancellationToken)
    {
        // اعتبارسنجی درخواست
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var animal = new Animal(command.Name.Trim());

         animal.AddDomainEvent(new AnimalCreatedDomainEvent(animal.Id, animal.Name));
        _animalRepository.Add(animal);

        await _animalRepository.UnitOfWork.SaveEntitiesAsync();
        await _mediator.Publish(new AnimalCreatedDomainEvent(animal.Id, animal.Name), cancellationToken);

        return Unit.Value;
    }
}
