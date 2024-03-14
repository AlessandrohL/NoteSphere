using Azure;
using Domain.Exceptions.BaseExceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Constants;
using WebApi.Helpers;

namespace WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                var response = context.Response;

                var errors = GetErrorsFromException(ex, out int statusCode);
                var title = HttpStatusHelper.GetTitleByStatusCode(statusCode);

                response.StatusCode = statusCode;

                var errorResponse = new ErrorResponseTwo(
                    title,
                    errors,
                    response.StatusCode);

                await response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static Dictionary<string, IEnumerable<string>> GetErrorsFromException(
            Exception ex, 
            out int statusCode)
        {
            switch (ex)
            {
                case BadRequestException e:
                    statusCode = StatusCodes.Status400BadRequest;
                    return e.Errors;

                case NotFoundException e:
                    statusCode = StatusCodes.Status404NotFound;
                    return e.Errors;

                case ValidationException e:
                    statusCode = StatusCodes.Status400BadRequest;
                    return e.Errors;

                case UnauthorizedException e:
                    statusCode = StatusCodes.Status401Unauthorized;
                    return e.Errors;

                case UnprocessableException e:
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    return e.Errors;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    return new() { { "Server", new string[] { "Internal Server Error" } } };
            }
        }
    }
}
