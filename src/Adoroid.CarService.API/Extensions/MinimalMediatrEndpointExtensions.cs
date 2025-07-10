using Adoroid.Core.Application.Exceptions.Types;
using Adoroid.Core.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.API.Extensions;

public static class MinimalMediatrEndpointExtensions
{
    public static RouteHandlerBuilder MinimalMediatrMapCommand<TRequest, TResponse>(
        this IEndpointRouteBuilder builder, string pattern, string httpMethod = "POST") where TRequest : IRequest<Response<TResponse>>
    {
        var handler = async ([FromBody] TRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await mediator.Send(request, cancellationToken);
                return result.ToResult();
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(e => new { field = e.Property ?? string.Empty, error = e.Errors });

                return Results.ValidationProblem(
                    errors.ToDictionary(e => e.field, e => e.error.ToArray())
                );
            }
        };

        return httpMethod.ToUpper() switch
        {
            "POST" => builder.MapPost(pattern, handler),
            "PUT" => builder.MapPut(pattern, handler),
            "DELETE" => builder.MapDelete(pattern, handler),
            _ => throw new NotSupportedException($"Http method not supported: {httpMethod}")
        };
    }
}
