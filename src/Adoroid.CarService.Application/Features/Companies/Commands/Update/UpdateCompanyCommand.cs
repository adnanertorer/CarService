using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Update
{
    public record UpdateCompanyCommand(Guid Id, string CompanyName, string AuthorizedName, string AuthorizedSurname,
    string TaxNumber, string TaxOffice, int CityId, int DistrictId, string CompanyAddress,
    string CompanyPhone, string CompanyEmail) : IRequest<Response<CompanyDto>>;

    public class UpdateCompanyCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateCompanyCommand, Response<CompanyDto>>
    {
        public async Task<Response<CompanyDto>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await dbContext.Companies.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (company == null)
                return Response<CompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

            company.UpdatedDate = DateTime.Now;
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
            
            dbContext.Update(company);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Response<CompanyDto>.Success(company.FromEntity());
        }
    }
}
