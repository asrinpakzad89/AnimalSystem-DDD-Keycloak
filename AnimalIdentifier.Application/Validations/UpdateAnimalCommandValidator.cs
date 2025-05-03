using AnimalIdentifier.Application.Commands;
using FluentValidation;

namespace AnimalIdentifier.Application.Validations;

public class UpdateAnimalCommandValidator : AbstractValidator<UpdateAnimalCommand>
{
    public UpdateAnimalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage("Animal id is required.");

        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Animal name is required.")
           .MinimumLength(2).WithMessage("Animal name must be at least 2 characters long.");

    }
}
