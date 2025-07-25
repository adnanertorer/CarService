﻿using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Vehicles.Commands.Create;
using Adoroid.CarService.Application.Features.Vehicles.Commands.Delete;
using Adoroid.CarService.Application.Features.Vehicles.Commands.Update;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.Queries.GetById;
using Adoroid.CarService.Application.Features.Vehicles.Queries.GetBySerialNumber;
using Adoroid.CarService.Application.Features.Vehicles.Queries.GetList;
using Adoroid.Core.Application.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class VehicleEndpointsMap
{
    private const string apiPath = "/api/vehicle";
    private const string mobileUserScheme = "MobileUser";

    public static IEndpointRouteBuilder VehicleEndpoint(this IEndpointRouteBuilder builder)
    {
        var schemes = new[] { JwtBearerDefaults.AuthenticationScheme, mobileUserScheme };

        builder.MinimalMediatrMapCommand<CreateVehicleCommand, VehicleDto>(apiPath)
            .RequireAuthorization(policy =>
                policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser()
            );

        builder.MinimalMediatrMapCommand<AssignVehicleToUserCommand, VehicleDto>(apiPath + "/assing-vehicle", "POST")
            .RequireAuthorization(policy =>
                policy.AddAuthenticationSchemes([mobileUserScheme]).RequireAuthenticatedUser()
            );

        builder.MinimalMediatrMapCommand<UpdateVehicleCommand, VehicleDto>(apiPath, "PUT").RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid vehicle id.");

            var result = await mediator.Send(new DeleteVehicleCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid vehicle id.");

            var result = await mediator.Send(new GetByIdVehicleRequest(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string customerId, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(customerId, out var guid))
                return Results.BadRequest("Invalid customer id.");

            var result = await mediator.Send(new GetListVehiclesQuery(pageRequest, guid, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme]).RequireAuthenticatedUser());

        builder.MapGet(apiPath + "/my-vehicles", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetMyVehicleListQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes([mobileUserScheme]).RequireAuthenticatedUser());

        builder.MapGet(apiPath + "/getby-serialnumber", async (string plateNumber, string serialNumber, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetBySerialNumberQuery(plateNumber, serialNumber), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(schemes).RequireAuthenticatedUser());

        return builder;
    }
}
