using Application.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Application.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An exception has been triggered: {Message}", exception.Message);
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Title = exception is ArgumentException ? "Bad Request" : "Server Error",
                Message = exception.Message,
                Details = exception.StackTrace
            };

            if (exception is ValidationException validationException)
            {
                errorResponse = new ErrorResponse
                {
                    StatusCode = statusCode,
                    Title = "Validation Error",
                    Message = validationException.Message,
                    Details = string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage))
                };
            }

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

}
