using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Create;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Delete;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Update;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetById;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class CompanyServiceEndpointsMap
{
    private const string apiPath = "/api/companyservices";
    public static IEndpointRouteBuilder CompanyServiceEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateCompanyServiceCommand, CompanyServiceDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateCompanyServiceCommand, CompanyServiceDto>(apiPath, "PUT").RequireAuthorization();

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid service id.");

            var result = await mediator.Send(new DeleteCompanyServiceCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid company service id.");

            var result = await mediator.Send(new GetByIdCompanyServiceQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetListCompanyServiceQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        });
        return builder;
    }
}
