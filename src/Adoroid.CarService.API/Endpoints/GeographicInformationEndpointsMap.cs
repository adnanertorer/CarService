using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Features.Cities.Queries.GetList;
using Adoroid.CarService.Application.Features.Districts.Queries.GetList;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Endpoints;

public static class GeographicInformationEndpointsMap
{
    private const string apiPath = "/api/geographicinfo";
    public static IEndpointRouteBuilder GeographicEndpoints(this IEndpointRouteBuilder builder)
    {
        var schemes = new[] { JwtBearerDefaults.AuthenticationScheme, "MobileUser" };

        builder.MapGet(apiPath + "/cities", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetListCityQuery(), cancellationToken);
            return result.ToResult();
        });

        builder.MapGet(apiPath + "/districts", async (int cityId, string? search, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetDistrictListQuery(cityId, search), cancellationToken);
            return result.ToResult();
        });

        return builder;
    }
}
