using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create;

public record CreateUserCommand(string Name, string Surname, string Email, string Password, Guid CompanyId, string PhoneNumber) :
    IRequest<Response<UserDto>>;

public class CreateUserCommandHandler(CarServiceDbContext dbContext, IAesEncryptionHelper aesEncryptionHelper) : IRequestHandler<CreateUserCommand, Response<UserDto>>
{
    public async Task<Response<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await dbContext.Users.AsNoTracking()
            .AnyAsync(i => i.Email == request.Email, cancellationToken);

        if(userExist )
            return Response<UserDto>.Fail(BusinessExceptionMessages.UserAlreadyExists);

        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = new User
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = encryptedPassword,
            CompanyId = request.CompanyId,
            PhoneNumber = request.PhoneNumber,
            CreatedBy = new Guid(),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        await dbContext.Users.AddAsync(user, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return Response<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            CompanyId = user.CompanyId,
            PhoneNumber = user.PhoneNumber
        });
    }
}
