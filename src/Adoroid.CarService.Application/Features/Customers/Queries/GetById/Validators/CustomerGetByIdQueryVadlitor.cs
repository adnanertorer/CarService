using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetById.Validators;

public class CustomerGetByIdQueryVadlitor : AbstractValidator<CustomerGetByIdQuery>
{
    public CustomerGetByIdQueryVadlitor()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
