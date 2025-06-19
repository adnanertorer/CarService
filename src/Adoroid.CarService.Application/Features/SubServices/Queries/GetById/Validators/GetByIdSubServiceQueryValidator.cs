using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetById.Validators;

public class GetByIdSubServiceQueryValidator : AbstractValidator<GetByIdSubServiceQuery>
{
    public GetByIdSubServiceQueryValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
