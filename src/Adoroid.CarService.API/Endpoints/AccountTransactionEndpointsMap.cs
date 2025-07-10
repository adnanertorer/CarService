using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Common.Dtos.Filters;
using Adoroid.CarService.Application.Features.AccountTransactions.Commands.Create;
using Adoroid.CarService.Application.Features.AccountTransactions.Commands.Update;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetById;
using Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetList;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class AccountTransactionEndpointsMap
{
    private const string apiPath = "/api/accounttransaction";

    public static IEndpointRouteBuilder AccountTransactionEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateClaimCommand, AccountTransactionDto>(apiPath).RequireAuthorization();
        builder.MapGet(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid transaction id.");

            var result = await mediator.Send(new GetByIdAccountTransactionRequest(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/adjustment/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid transaction id.");

            var result = await mediator.Send(new AdjustmentAccountTransactionCommand(guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/list", async ([AsParameters] PageRequest pageRequest, string? search, DateTime? startDate, DateTime? endDate, string? customerId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            Guid? cId = null;
            if (!string.IsNullOrEmpty(customerId))
            {
                if (!Guid.TryParse(customerId, out var id))
                    return Results.BadRequest("Invalid customer id.");
                cId = id;
            }
            var result = await mediator.Send(new GetListAccountTransactionRequest(new MainFilterRequestModel(pageRequest, search, startDate, endDate, cId)), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();
        return builder;
    }
}
