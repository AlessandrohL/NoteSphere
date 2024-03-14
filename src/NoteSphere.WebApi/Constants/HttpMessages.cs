namespace WebApi.Constants
{
    public static class HttpMessages
    {
        public const string Ok = "Operation completed successfully.";
        public const string Created = "Resource created successfully.";
        public const string BadRequest = "The request is incorrect or malformed.";
        public const string NotFound = "The requested resource was not found.";
        public const string Forbidden = "You do not have permission to access the resource.";
        public const string Unauthorized = "Authentication is required to access the resource.";
        public const string InternalServerError = "An internal server error occurred.";
        public const string UnprocessableEntity = "The entity could not be processed.";
    }
}
