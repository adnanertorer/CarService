using Adoroid.CarService.Application.Common.ValidationMessages;
using FluentValidation;

namespace Adoroid.CarService.Application.Features.MasterServices.Queries.GetById.Validators;

public class GetByIdMasterServiceQueryValidator : AbstractValidator<GetByIdMasterServiceQuery>
{
    public GetByIdMasterServiceQueryValidator()
    {
        RuleFor(x => x.Id)
          .NotNull()
          .WithMessage(string.Format(ValidationMessages.Required, "Id"));
    }
}
