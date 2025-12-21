namespace TicketingSystem.API.Middlewares;

public class RequestLoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var start = DateTime.UtcNow;

        await next(context);

        var duration = DateTime.UtcNow - start;

        Console.WriteLine(
            $"Path: {context.Request.Path}, " +
            $"Status: {context.Response.StatusCode}, " +
            $"Duration: {duration.TotalMilliseconds}ms");
    }
}