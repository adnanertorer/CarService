using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetList.Validations;

public class SupplierGetListQueryValidator : AbstractValidator<SupplierGetListQuery>
{
    public SupplierGetListQueryValidator()
    {
        RuleFor(i => i.PageRequest).NotNull()
       .WithMessage(ValidationMessages.PageRequestRequired);

        RuleFor(i => i.PageRequest.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeGreaterThanZero)
            .LessThan(100)
            .WithMessage(ValidationMessages.PageRequestPageSizeMustBeLessThan100);
    }
}
