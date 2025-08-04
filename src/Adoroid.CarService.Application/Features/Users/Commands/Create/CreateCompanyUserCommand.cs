using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;
using UserDto = Adoroid.CarService.Application.Features.Users.Dtos.UserDto;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create;

public record CreateCompanyUserCommand(CreateUserDto CreateUserDto, CreateCompanyDto CreateCompanyDto) : IRequest<Response<UserDto>>;

public class CreateCompanyUserCommandHandler(IUnitOfWork unitOfWork, IAesEncryptionHelper aesEncryptionHelper, ILogger<CreateCompanyUserCommandHandler> logger) : IRequestHandler<CreateCompanyUserCommand, Response<UserDto>>
{
    public async Task<Response<UserDto>> Handle(CreateCompanyUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await unitOfWork.Users.AnyUserWithEmailAndPhonenumber(request.CreateUserDto.Email,
            request.CreateUserDto.PhoneNumber, cancellationToken);

        if (userExist)
            return Response<UserDto>.Fail(BusinessExceptionMessages.UserAlreadyExists);

        var encryptedPassword = aesEncryptionHelper.Encrypt(request.CreateUserDto.Password);
        var user = new User
        {
            Name = request.CreateUserDto.Name,
            Surname = request.CreateUserDto.Surname,
            Email = request.CreateUserDto.Email,
            Password = encryptedPassword,
            PhoneNumber = request.CreateUserDto.PhoneNumber,
            CreatedBy = new Guid(),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User created with ID: {UserId}", user.Id);

        var isExist = await unitOfWork.Companies.IsCompanyExistsAsync(request.CreateCompanyDto.TaxNumber,
            request.CreateCompanyDto.CompanyEmail, cancellationToken);

        if (isExist)
            return Response<UserDto>.Fail(BusinessExceptionMessages.CompanyAlreadyExists);

        var company = new Company
        {
            AuthorizedName = request.CreateCompanyDto.AuthorizedName,
            AuthorizedSurname = request.CreateCompanyDto.AuthorizedSurname,
            CityId = request.CreateCompanyDto.CityId,
            CompanyAddress = request.CreateCompanyDto.CompanyAddress,
            CompanyEmail = request.CreateCompanyDto.CompanyEmail,
            CompanyName = request.CreateCompanyDto.CompanyName,
            CompanyPhone = request.CreateCompanyDto.CompanyPhone,
            CreatedBy = user.Id,
            CreatedDate = DateTime.UtcNow,
            DistrictId = request.CreateCompanyDto.DistrictId,
            IsDeleted = false,
            TaxNumber = request.CreateCompanyDto.TaxNumber,
            TaxOffice = request.CreateCompanyDto.TaxOffice
        };

        await unitOfWork.Companies.AddAsync(company, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Company created with ID: {CompanyId}", company.Id);

        var isExistCompanyUser = await unitOfWork.UserToCompanies.IsExists(user.Id, company.Id, cancellationToken);

        if (isExistCompanyUser)
            return Response<UserDto>.Fail(BusinessExceptionMessages.AlreadyExistsCompanyUser);

        var entity = new UserToCompany
        {
            CompanyId = company.Id,
            IsDeleted = false,
            UserId = user.Id,
            CreatedBy = user.Id,
            CreatedDate = DateTime.UtcNow,
            UserType = (int)CompanyUserTypeEnum.Master
        };

        await unitOfWork.UserToCompanies.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User to Company relation created with UserId: {UserId} and CompanyId: {CompanyId}", user.Id, company.Id);

        return Response<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });
    }
}
