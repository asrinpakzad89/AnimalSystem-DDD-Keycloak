using AnimalIdentifier.Application.Commands;
using FluentValidation;

namespace AnimalIdentifier.Application.Validations;

public class UpdateAnimalCommandValidator : AbstractValidator<UpdateAnimalCommand>
{
    public UpdateAnimalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
    }
}
