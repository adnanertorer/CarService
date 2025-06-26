using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Application.Features.MobileUsers.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MobileUsers.Commands.Create;

public record CreateMobilUserCommand(string Name, string Surname, string Email, string Password, string PhoneNumber) 
    : IRequest<Response<MobileUserDto>>;

public class CreateMobilUserCommandHandler(CarServiceDbContext dbContext, IAesEncryptionHelper aesEncryptionHelper)
    : IRequestHandler<CreateMobilUserCommand, Response<MobileUserDto>>
{
    public async Task<Response<MobileUserDto>> Handle(CreateMobilUserCommand request, CancellationToken cancellationToken)
    {
        var isExist = await dbContext.MobileUsers
            .AsNoTracking()
            .Where(i => i.Email == request.Email)
            .AnyAsync(cancellationToken);

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
            IsDeleted = false
        };

        await dbContext.MobileUsers.AddAsync(user, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

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

