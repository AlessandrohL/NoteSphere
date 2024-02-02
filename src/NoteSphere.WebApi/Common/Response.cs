namespace WebApi.Common
{
    public abstract class Response
    {
        public int Status { get; set; }
        public string? Title { get; set; }
        public bool IsSuccess { get; set; }
    }
}
