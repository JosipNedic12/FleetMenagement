using System.Text.Json;
using FleetManagement.Application.Exceptions;

namespace FleetManagement.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteResponse(context, 404, ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteResponse(context, 409, ex.Message);
        }
        catch (Exception)
        {
            await WriteResponse(context, 500, "An unexpected error occurred.");
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var body = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(body);
    }
}
