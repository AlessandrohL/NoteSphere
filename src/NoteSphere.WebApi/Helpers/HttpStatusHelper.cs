using WebApi.Constants;

namespace WebApi.Helpers
{
    public static class HttpStatusHelper
    {
        public static string GetTitleByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status200OK => HttpMessages.Ok,
                StatusCodes.Status201Created => HttpMessages.Created,
                StatusCodes.Status204NoContent => HttpMessages.Ok,
                StatusCodes.Status400BadRequest => HttpMessages.BadRequest,
                StatusCodes.Status404NotFound => HttpMessages.NotFound,
                StatusCodes.Status403Forbidden => HttpMessages.Forbidden,
                StatusCodes.Status401Unauthorized => HttpMessages.Unauthorized,
                StatusCodes.Status422UnprocessableEntity => HttpMessages.UnprocessableEntity,
                StatusCodes.Status500InternalServerError => HttpMessages.InternalServerError,
                _ => "Unknown error."
            };
        }
    }
}
