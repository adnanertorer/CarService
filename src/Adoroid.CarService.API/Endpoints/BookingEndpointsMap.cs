using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Bookings.Commands.Create;
using Adoroid.CarService.Application.Features.Bookings.Commands.Delete;
using Adoroid.CarService.Application.Features.Bookings.Commands.Update;
using Adoroid.CarService.Application.Features.Bookings.Dtos;
using Adoroid.CarService.Application.Features.Bookings.Qeries.GetByCompanyId;
using Adoroid.CarService.Application.Features.Bookings.Qeries.GetByMobileUserIdQuery;
using Adoroid.Core.Application.Requests;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class BookingEndpointsMap
{
    private const string apiPath = "/api/booking";
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<CreateBookingCommand, BookingDto>(apiPath, "POST");
        builder.MinimalMediatrMapCommand<ApproveBookingCommand, BookingDto>(apiPath, "PUT").RequireAuthorization();

        builder.MapDelete(apiPath + "/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(id, out var guid))
                return Results.BadRequest("Invalid booking id.");

            var result = await mediator.Send(new DeleteBookingCommand(guid), cancellationToken);
            return result.ToResult();

        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(["MobileUser"]).RequireAuthenticatedUser());

        builder.MapGet(apiPath + "/get-by-company", async ([AsParameters] PageRequest pageRequest, string companyId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(companyId, out var guid))
                return Results.BadRequest("Invalid company id.");

            var result = await mediator.Send(new GetByCompanyIdAsyncQuery(pageRequest, guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization();

        builder.MapGet(apiPath + "/get-by-mobileuser", async ([AsParameters] PageRequest pageRequest, string mobileUserId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(mobileUserId, out var guid))
                return Results.BadRequest("Invalid mobile user id.");

            var result = await mediator.Send(new GetByMobileUserIdAsyncQuery(pageRequest, guid), cancellationToken);
            return result.ToResult();
        }).RequireAuthorization(policy =>
        policy.AddAuthenticationSchemes(["MobileUser"]).RequireAuthenticatedUser());

        return builder;
    }
}
