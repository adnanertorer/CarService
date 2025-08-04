using Adoroid.CarService.Application.Features.Users.Dtos;

namespace Adoroid.CarService.Application.Features.Users.Commands.Create;

public record CreateCompanyUserRequest(CreateUserDto User, CreateCompanyDto Company);
