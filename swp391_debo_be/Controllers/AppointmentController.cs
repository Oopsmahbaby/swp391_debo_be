using Google.Apis.Oauth2.v2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    [Route("api")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ITokenService _tokenService;

        public AppointmentController(IAppointmentService appointmentService, ITokenService tokenService)
        {
            this._appointmentService = appointmentService;
            _tokenService = tokenService;
        }

        [HttpGet("patient/calendar")]
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDate([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string view)
        {
            var authHeader = _tokenService.GetAuthorizationHeader(Request);

            if (string.IsNullOrEmpty(authHeader))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            var token = authHeader.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return new ApiRespone { Data = null, Message = "Token is required", Success = false };
            }

            var userId = _tokenService.GetUserIdFromToken(token);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Invalid token", Success = false };
            }

            return _appointmentService.GetAppointmentsByStartDateAndEndDate(startDate, endDate ,userId);

        }

        [HttpGet("patient/appointments")]
        public ActionResult<ApiRespone> GetAppointmentsByPagination([FromQuery] string page, [FromQuery] string limit)
        {
            var authHeader = _tokenService.GetAuthorizationHeader(Request);

            if (string.IsNullOrEmpty(authHeader))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            var token = authHeader.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return new ApiRespone { Data = null, Message = "Token is required", Success = false };
            }

            var userId = _tokenService.GetUserIdFromToken(token);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Invalid token", Success = false };
            }

            return _appointmentService.GetAppointmentByPagination(page, limit, userId);
        }
    }
}
