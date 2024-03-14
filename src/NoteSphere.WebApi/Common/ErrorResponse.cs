using System.Collections;

namespace WebApi.Common
{
    public class ErrorResponse: Response
    {
        public IEnumerable<string> Errors { get; set; }

        public ErrorResponse(string title, IEnumerable<string> errors, int statusCode)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = false;
            Errors = errors;
        }

        public ErrorResponse(string title, string error, int statusCode)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = false;
            Errors = new string[] { error };
        }
    }

    public sealed class ErrorResponseTwo : Response
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; init; }

        public ErrorResponseTwo(string title, Dictionary<string, IEnumerable<string>> errorDic, int statusCode)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = false;
            Errors = errorDic;
        }
    }
}
