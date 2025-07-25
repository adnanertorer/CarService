﻿using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Delete.Validators;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
