using WebApi.Constants;
using WebApi.Helpers;

namespace WebApi.Common
{
    public class SuccessResponse<T> : Response
    {
        public T? Result { get; set; }

        private SuccessResponse()
        { }

        public SuccessResponse(T data)
        {
            Status = StatusCodes.Status200OK;
            Title = HttpMessages.Ok;
            IsSuccess = true;
            Result = data;
        }

        public static SuccessResponse<T> Created(T? data)
        {
            return new SuccessResponse<T>
            {
                Status = StatusCodes.Status201Created,
                Title = HttpStatusHelper
                    .GetTitleByStatusCode(StatusCodes.Status201Created),
                IsSuccess = true,
                Result = data
            };
        }

        public static SuccessResponse<T> Ok(T? data)
        {
            return new SuccessResponse<T>
            {
                Status = StatusCodes.Status200OK,
                Title = HttpStatusHelper
                    .GetTitleByStatusCode(StatusCodes.Status200OK),
                IsSuccess = true,
                Result = data
            };
        }

        public static SuccessResponse<bool> NoContent()
        {
            return new SuccessResponse<bool>
            {
                Status = StatusCodes.Status204NoContent,
                Title = HttpStatusHelper
                    .GetTitleByStatusCode(StatusCodes.Status204NoContent),
                IsSuccess = true,
                Result = true
            };
        }
    }
}
