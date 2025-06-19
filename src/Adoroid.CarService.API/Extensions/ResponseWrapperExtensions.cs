using Adoroid.Core.Application.Wrappers;

namespace Adoroid.CarService.API.Extensions;

public static class ResponseWrapperExtensions
{
    public static IResult ToResult<T>(this Response<T> response)
    {
        if (response.Succeeded)
            return Results.Ok(response);
        return Results.Problem(response.Message, statusCode: response.StatusCode);
    }
}
