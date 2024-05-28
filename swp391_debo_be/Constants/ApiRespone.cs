using System.Net;

namespace swp391_debo_be.Constants
{
    public class ApiRespone
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public object? Result { get; set; }
    }
}
