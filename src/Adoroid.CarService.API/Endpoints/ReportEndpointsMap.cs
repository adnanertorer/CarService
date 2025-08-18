using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Reports.Queries.GetHighestEarningCustomers;
using Adoroid.CarService.Application.Features.Reports.Queries.GetServiceCountByEmployee;
using Adoroid.CarService.Application.Features.Reports.Queries.GetTransactions;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class ReportEndpointsMap
{
    private const string apiPath = "/api/reports";
    public static IEndpointRouteBuilder ReportEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(apiPath + "/transactions", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetTransactionQuery(), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/highest-earning", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetHighestEarningCustomersQuery(), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/employee-service-count", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetServiceCountByEmployeeQuery(), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        return builder;
    }
}
