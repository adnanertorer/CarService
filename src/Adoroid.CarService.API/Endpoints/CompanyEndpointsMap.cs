using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Companies.Commands.Create;
using Adoroid.CarService.Application.Features.Companies.Commands.Delete;
using Adoroid.CarService.Application.Features.Companies.Commands.Update;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.Queries.GetById;
using Adoroid.CarService.Application.Features.Companies.Queries.GetList;
using Adoroid.Core.Application.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class CompanyEndpointsMap
{
    private const string apiPath = "/api/company";
    public static IEndpointRouteBuilder CompanyEndpoints(this IEndpointRouteBuilder builder)
    {
        var schemes = new[] { JwtBearerDefaults.AuthenticationScheme, "MobileUser" };

        builder.MinimalMediatrMapCommand<CreateCompanyCommand, CompanyDto>(apiPath, "POST");
        builder.MinimalMediatrMapCommand<UpdateCompanyCommand, CompanyDto>(apiPath, "PUT").RequireAuthorization();

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid company id.");

            var result = await mediator.Send(new DeleteCompanyCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid company id.");

            var result = await mediator.Send(new CompanyGetByIdQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, int? cityId, int? districtId, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new CompanyGetListQuery(pageRequest, cityId, districtId, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
                policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser()
            );
        return builder;
    }
}
