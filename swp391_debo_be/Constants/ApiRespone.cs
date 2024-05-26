using System.Net;

namespace swp391_debo_be.Constants
{
    public class ApiRespone
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; } = new List<string> { "No Error Found." };
        public object? Result { get; set; }
    }
}
