using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Shared.AspNetCore.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Ruft die nächste Middleware in der Pipeline auf.
                await next(context);
            }
            catch (Exception ex)
            {
                // Fängt jede Exception ab und behandelt sie.
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            object response;

            // Wandelt spezifische Exception-Typen in passende HTTP-Statuscodes um.
            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new
                    {
                        title = "Validation Error",
                        status = (int)statusCode,
                        errors = validationException.Errors
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new
                    {
                        title = "Not Found",
                        status = (int)statusCode,
                        detail = notFoundException.Message
                    };
                    break;

                case ForbiddenAccessException forbiddenAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    response = new
                    {
                        title = "Forbidden",
                        status = (int)statusCode,
                        detail = forbiddenAccessException.Message
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = new
                    {
                        title = "Internal Server Error",
                        status = (int)statusCode,
                        detail = "An unexpected error occurred. Please try again later."
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
