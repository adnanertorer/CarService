using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Customers.Commands.Create;
using Adoroid.CarService.Application.Features.Customers.Commands.Delete;
using Adoroid.CarService.Application.Features.Customers.Commands.Update;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.Queries.GetById;
using Adoroid.CarService.Application.Features.Customers.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class CustomerEndpointMap
{
    private const string apiPath = "/api/customer";

    public static IEndpointRouteBuilder CustomerEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateCustomerCommand, CustomerDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateCustomerCommand, CustomerDto>(apiPath, "PUT").RequireAuthorization();
        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid customer id.");

            var result = await mediator.Send(new DeleteCustomerCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid customer id.");

            var result = await mediator.Send(new CustomerGetByIdQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetCustomerListQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
