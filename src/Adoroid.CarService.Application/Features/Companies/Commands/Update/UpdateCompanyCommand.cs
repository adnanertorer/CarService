using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Update
{
    public record UpdateCompanyCommand(Guid Id, string CompanyName, string AuthorizedName, string AuthorizedSurname,
    string TaxNumber, string TaxOffice, int CityId, int DistrictId, string CompanyAddress,
    string CompanyPhone, string CompanyEmail) : IRequest<Response<CompanyDto>>;

    public class UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateCompanyCommand, Response<CompanyDto>>
    {
        public async Task<Response<CompanyDto>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await unitOfWork.Companies.GetByIdAsync(request.Id, cancellationToken);
            if (company == null)
                return Response<CompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

            company.UpdatedDate = DateTime.UtcNow;
            company.UpdatedBy = Guid.Parse(currentUser.Id!);
            company.TaxOffice = request.TaxOffice;
            company.CityId = request.CityId;
            company.AuthorizedName = request.AuthorizedName;
            company.AuthorizedSurname = request.AuthorizedSurname;
            company.CompanyPhone = request.CompanyPhone;
            company.CompanyAddress = request.CompanyAddress;
            company.CompanyEmail = request.CompanyEmail;
            company.CompanyName = request.CompanyName;  
            company.DistrictId = request.DistrictId;
            company.TaxNumber = request.TaxNumber;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<CompanyDto>.Success(company.FromEntity());
        }
    }
}
