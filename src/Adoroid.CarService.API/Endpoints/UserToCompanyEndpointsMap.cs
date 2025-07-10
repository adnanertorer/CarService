using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.UserToCompanies.Commands.Create;
using Adoroid.CarService.Application.Features.UserToCompanies.Commands.Delete;
using Adoroid.CarService.Application.Features.UserToCompanies.Commands.Update;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetById;
using Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetList;
using Adoroid.Core.Application.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class UserToCompanyEndpointsMap
{
    private const string apiPath = "/api/usertocompany";
    public static IEndpointRouteBuilder UserToCompanyEndpoints(this IEndpointRouteBuilder builder)
    {
        var schemes = new[] { JwtBearerDefaults.AuthenticationScheme, "MobileUser" };

        builder.MinimalMediatrMapCommand<CreateUserToCompanyCommand, UserToCompanyDto>(apiPath)
           .RequireAuthorization(policy =>
               policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser()
           );

        builder.MinimalMediatrMapCommand<UpdateUserToCompanyCommand, UserToCompanyDto>(apiPath, "PUT").RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid id.");

            var result = await mediator.Send(new DeleteUserToCompanyCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid vehicle id.");

            var result = await mediator.Send(new GetByIdUserToCompanyQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, IMediator mediator, CancellationToken cancellationToken) =>
        {

            var result = await mediator.Send(new GetListUserToCompanyQuery(pageRequest), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        return builder;
    }
}
