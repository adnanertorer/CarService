using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Delete.Validations;

public class DeleteSupplierCommandVadlitor : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierCommandVadlitor()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
