﻿using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Application.Features.MobileUsers.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MobileUsers.Commands.Create;

public record CreateMobilUserCommand(string Name, string Surname, string Email, string Password, string PhoneNumber, int CityId, int DistrictId,
    string? Address) 
    : IRequest<Response<MobileUserDto>>;

public class CreateMobilUserCommandHandler(IUnitOfWork unitOfWork, IAesEncryptionHelper aesEncryptionHelper)
    : IRequestHandler<CreateMobilUserCommand, Response<MobileUserDto>>
{
    public async Task<Response<MobileUserDto>> Handle(CreateMobilUserCommand request, CancellationToken cancellationToken)
    {
        var isExist = await unitOfWork.MobileUsers.IsExistByEmail(request.Email, cancellationToken);

        if (isExist)
            return Response<MobileUserDto>.Fail(BusinessExceptionMessages.UserAlreadyExists);

        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = new MobileUser
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = encryptedPassword,
            PhoneNumber = request.PhoneNumber,
            CreatedBy = new Guid(),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false,
            CityId = request.CityId,
            DistrictId = request.DistrictId,
            Address = request.Address
        };

        var _ = await unitOfWork.MobileUsers.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<MobileUserDto>.Success(new MobileUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });
    }
}

