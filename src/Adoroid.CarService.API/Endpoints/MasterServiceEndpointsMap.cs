using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.MasterServices.Commands.Create;
using Adoroid.CarService.Application.Features.MasterServices.Commands.Delete;
using Adoroid.CarService.Application.Features.MasterServices.Commands.Update;
using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.Queries.GetById;
using Adoroid.CarService.Application.Features.MasterServices.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class MasterServiceEndpointsMap
{
    private const string apiPath = "/api/masterservice";
    public static IEndpointRouteBuilder MasterServiceEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateMasterServiceCommand, MasterServiceDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateMasterServiceCommand, MasterServiceDto>(apiPath, "PUT").RequireAuthorization();
        builder.MinimalMediatrMapCommand<DeleteMasterServiceCommand, Guid>(apiPath, "DELETE").RequireAuthorization();
        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid master service id.");

            var result = await mediator.Send(new GetByIdMasterServiceQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetListMasterServicesQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
