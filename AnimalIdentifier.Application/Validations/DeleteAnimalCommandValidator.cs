using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalIdentifier.Application.Commands;
using FluentValidation;

namespace AnimalIdentifier.Application.Validations;

public class DeleteAnimalCommandValidator :AbstractValidator<DeleteAnimalCommand>
{
    public DeleteAnimalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
           .WithMessage("Animal id is required.");
    }
}
