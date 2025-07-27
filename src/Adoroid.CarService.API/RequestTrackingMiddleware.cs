public class RequestTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTrackingMiddleware> _logger;

    public RequestTrackingMiddleware(RequestDelegate next, ILogger<RequestTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;

        _logger.LogInformation("➡️ [REQ] {method} {path}", method, path);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "🔥 Unhandled exception during request pipeline: {method} {path}", method, path);
            throw;
        }

        _logger.LogInformation("✅ [RES] {method} {path} → {status}", method, path, context.Response.StatusCode);
    }
}
