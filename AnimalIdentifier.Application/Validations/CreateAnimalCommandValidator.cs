using AnimalIdentifier.Application.Commands;
using FluentValidation;

namespace AnimalIdentifier.Application.Validations;

public class CreateAnimalCommandValidator : AbstractValidator<CreateAnimalCommand>
{
    public CreateAnimalCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Animal name is required.")
            .MinimumLength(3).WithMessage("Animal name must be at least 3 characters long.");
    }
}
