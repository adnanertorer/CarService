using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Commands.Create;
using Adoroid.CarService.Application.Features.SubServices.Commands.Delete;
using Adoroid.CarService.Application.Features.SubServices.Commands.Update;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.Queries.GetById;
using Adoroid.CarService.Application.Features.SubServices.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class SubServiceEnpointsMap
{
    private const string apiPath = "/api/subservice";

    public static IEndpointRouteBuilder SubServiceEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateSubServiceCommand, SubServiceDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateSubServiceCommand, SubServiceDto>(apiPath, "PUT").RequireAuthorization();
        builder.MinimalMediatrMapCommand<DeleteSubServiceCommand, Guid>(apiPath, "DELETE").RequireAuthorization();
        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid sub-service id.");

            var result = await mediator.Send(new GetByIdSubServiceQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string mainServiceId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(mainServiceId, out var guid))
                return Results.BadRequest("Invalid main-service id.");

            var result = await mediator.Send(new GetListSubServiceQuery(pageRequest, guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
