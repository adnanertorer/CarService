using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Delete.Validators;

public class DeleteMasterServiceCommandValidator : AbstractValidator<DeleteMasterServiceCommand>
{
    public DeleteMasterServiceCommandValidator()
    {
        RuleFor(x => x.Id)
           .NotNull()
           .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
