using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Suppliers.Commands.Create;
using Adoroid.CarService.Application.Features.Suppliers.Commands.Delete;
using Adoroid.CarService.Application.Features.Suppliers.Commands.Update;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.Queries.GetById;
using Adoroid.CarService.Application.Features.Suppliers.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class SupplierEnpointsMap
{
    private const string apiPath = "/api/supplier";

    public static IEndpointRouteBuilder SupplierEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateSupplierCommand, SupplierDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateSupplierCommand, SupplierDto>(apiPath, "PUT").RequireAuthorization();

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid suplier id.");

            var result = await mediator.Send(new DeleteSupplierCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid supplier id.");

            var result = await mediator.Send(new SupplierGetByIdRequest(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new SupplierGetListQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
