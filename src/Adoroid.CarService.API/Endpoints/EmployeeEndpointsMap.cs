using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Employees.Commands.Create;
using Adoroid.CarService.Application.Features.Employees.Commands.Delete;
using Adoroid.CarService.Application.Features.Employees.Commands.Update;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.Queries.GetById;
using Adoroid.CarService.Application.Features.Employees.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class EmployeeEndpointsMap
{
    private const string apiPath = "/api/employee";
    public static IEndpointRouteBuilder EmployeeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateEmployeeCommand, EmployeeDto>(apiPath).RequireAuthorization();
        builder.MinimalMediatrMapCommand<UpdateEmployeeCommand, EmployeeDto>(apiPath, "PUT").RequireAuthorization();
        builder.MinimalMediatrMapCommand<DeleteEmployeeCommand, Guid>(apiPath, "DELETE").RequireAuthorization();
        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid company ID.");

            var result = await mediator.Send(new EmployeeGetByIdQuery(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetEmployeeListQuery(pageRequest, search), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
