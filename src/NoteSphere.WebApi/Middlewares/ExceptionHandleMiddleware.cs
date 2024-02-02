using Azure;
using Domain.Exceptions.BaseExceptions;
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
                response.ContentType = "application/json";

                var errors = GetErrorMessages(ex, out int statusCode);
                response.StatusCode = statusCode;

                var title = HttpStatusHelper.GetTitleByStatusCode(statusCode);

                var errorResponse = new ErrorResponse(
                    title,
                    errors,
                    response.StatusCode);

                await response.WriteAsJsonAsync(errorResponse);

            }
        }

        private List<string> GetErrorMessages(Exception ex, out int statusCode)
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

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    return new() { "Internal Server Error" };
            }
        }
    }
}
