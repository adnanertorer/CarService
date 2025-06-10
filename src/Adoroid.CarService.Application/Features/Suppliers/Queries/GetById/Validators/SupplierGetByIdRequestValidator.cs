using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetById.Validators;

public class SupplierGetByIdRequestValidator : AbstractValidator<SupplierGetByIdRequest>
{
    public SupplierGetByIdRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
