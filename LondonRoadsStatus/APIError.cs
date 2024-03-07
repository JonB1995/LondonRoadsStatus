namespace LondonRoadStatus
{
    public class APIError
    {
        public string? TimestampUTC { get; set; }
        public string? ExceptionType { get; set; }
        public int? HTTPStatusCode { get; set; }
        public string? HTTPStatus { get; set; }
        public string? Message { get; set; }
    }
}