namespace Application.Models.Response
{
    internal class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? Details { get; set; }
    }
}
