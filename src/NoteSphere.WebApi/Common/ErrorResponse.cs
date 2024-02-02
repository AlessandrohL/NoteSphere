
namespace WebApi.Common
{
    public class ErrorResponse: Response
    {
        public List<string> Errors { get; set; }

        public ErrorResponse(string title, List<string> errors, int statusCode)
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
            Errors = new() { error };
        }
    }
}
