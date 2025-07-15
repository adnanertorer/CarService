using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create;

public record CreateUserCommand(string Name, string Surname, string Email, string Password, string PhoneNumber) :
    IRequest<Response<UserDto>>;

public class CreateUserCommandHandler(IUnitOfWork unitOfWork, IAesEncryptionHelper aesEncryptionHelper) : IRequestHandler<CreateUserCommand, Response<UserDto>>
{
    public async Task<Response<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await unitOfWork.Users.AnyUserWithEmailAndPhonenumber(request.Email, request.PhoneNumber, cancellationToken);

        if (userExist )
            return Response<UserDto>.Fail(BusinessExceptionMessages.UserAlreadyExists);

        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = new User
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

        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
