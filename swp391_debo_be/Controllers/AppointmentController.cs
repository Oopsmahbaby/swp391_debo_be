﻿using Google.Apis.Oauth2.v2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
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
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDate([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string view)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.GetAppointmentsByStartDateAndEndDate(startDate, endDate, userId);

        }

        [HttpGet("patient/appointments")]
        public ActionResult<ApiRespone> GetAppointmentsByPagination([FromQuery] string page, [FromQuery] string limit)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.GetAppointmentByPagination(page, limit, userId);
        }

        [HttpGet("slot")]
        public ActionResult<ApiRespone> GetApppointmentsByDentistIdAndDate([FromQuery] string dentist, [FromQuery] string date)
        {
            return _appointmentService.GetApppointmentsByDentistIdAndDate(dentist, date);
        }

        [HttpPost("appointment")]
        public ActionResult<ApiRespone> CreateAppointment([FromBody] AppointmentDto dto)
        {
            string role = JwtProvider.GetRole(Request);

            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(role))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.CreateAppointment(dto, role, userId);
        }

        [HttpDelete("appointment/{id}")]
        public ActionResult<ApiRespone> CancelAppointment([FromRoute] string id)
        {

            return _appointmentService.CancelAppointment(id);
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

        [HttpGet("viewhistoryappoinment")]
        public async Task<IActionResult> GetHistoryAppointmentByUserID(Guid id)
        {
            var response = await _appointmentService.GetHistoryAppointmentByUserID(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("viewallappointment")]
        public async Task<IActionResult> ViewAllAppointment([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _appointmentService.ViewAllAppointment(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("dentist/calendar")]
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDateOfDentist([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string view)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.GetAppointmentsByStartDateAndEndDateOfDentist(startDate, endDate, userId);

        }
    }
}
