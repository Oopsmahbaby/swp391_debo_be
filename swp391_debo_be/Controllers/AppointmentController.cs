using Google.Apis.Oauth2.v2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
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
            _appointmentService = appointmentService;
            _tokenService = tokenService;
        }

        [HttpGet("patient/calendar")]
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDate([FromQuery] string start, [FromQuery] string end, [FromQuery] string view)
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

            return _appointmentService.GetAppointmentsByStartDateAndEndDate(start, end ,userId);

        }

        [HttpGet("patient/appointments")]
        public ActionResult<ApiRespone> GetAppointmentsByPagination([FromQuery] string page, [FromQuery] string limit)
        {
            var result = CheckAuthorizationHeader();

            if (result is ActionResult<ApiRespone>)
            {
                return (ActionResult<ApiRespone>)result;
            }

            return _appointmentService.GetAppointmentByPagination(page, limit, (string)result);
        }

        [HttpGet("slot")]
        public ActionResult<ApiRespone> GetApppointmentsByDentistIdAndDate([FromQuery] string dentist, [FromQuery] string date)
        {
            return _appointmentService.GetApppointmentsByDentistIdAndDate(dentist, date);
        }

        [HttpPost("appointment")]
        public ActionResult<ApiRespone> CreateAppointment([FromBody] AppointmentDto dto)
        {
            var result = CheckAuthorizationHeader();

            if (result is ActionResult<ApiRespone>)
            {
                return (ActionResult<ApiRespone>)result;
            }

            return _appointmentService.CreateAppointment(dto, result);
        }

        private object CheckAuthorizationHeader()
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

            return userId;
        }
    }
}
