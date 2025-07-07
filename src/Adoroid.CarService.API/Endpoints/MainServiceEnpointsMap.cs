using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Common.Dtos.Filters;
using Adoroid.CarService.Application.Features.MainServices.Commands.Create;
using Adoroid.CarService.Application.Features.MainServices.Commands.Delete;
using Adoroid.CarService.Application.Features.MainServices.Commands.Update;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.Queries.GetById;
using Adoroid.CarService.Application.Features.MainServices.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class MainServiceEnpointsMap
{
    private const string apiPath = "/api/mainservice";
    private const string mobileUserScheme = "MobileUser";

    public static IEndpointRouteBuilder MainServiceEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateMainServiceCommand, MainServiceDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateMainServiceCommand, MainServiceDto>(apiPath, "PUT").RequireAuthorization();
        builder.MinimalMediatrMapCommand<DeleteMainServiceCommand, Guid>(apiPath, "DELETE").RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid service id.");

            var result = await mediator.Send(new GetByIdMainServiceQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, DateTime? startDate, DateTime? endDate, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetListMainServiceQuery(new MainFilterRequestModel(pageRequest, search, startDate, endDate, null)), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/my-services", async ([AsParameters] PageRequest pageRequest, string vehicleId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(vehicleId, out var guid))
                return Results.BadRequest("Invalid vehicle id.");

            var result = await mediator.Send(new GetListMainServiceByMyVehicleQuery(pageRequest, guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes([mobileUserScheme]).RequireAuthenticatedUser());

        return builder;
    }
}
