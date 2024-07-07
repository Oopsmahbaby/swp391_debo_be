﻿using Azure;
using Google.Apis.Oauth2.v2;
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
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDate([FromQuery] string start, [FromQuery] string end, [FromQuery] string view)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.GetAppointmentsByStartDateAndEndDate(start, end, userId);

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
        public ActionResult<ApiRespone> GetApppointmentsByDentistIdAndDate([FromQuery] string dentist, [FromQuery] string date, [FromQuery] string treatment)
        {
            return _appointmentService.GetApppointmentsByDentistIdAndDate(dentist, date, treatment);
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

            return _appointmentService.CreateAppointment(dto, userId, role);
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

        [HttpGet("viewhistoryappoinment/{id}")]
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
        public ActionResult<ApiRespone> GetAppointmentsByStartDateAndEndDateOfDentist([FromQuery] string start, [FromQuery] string end, [FromQuery] string view)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }

            return _appointmentService.GetAppointmentsByStartDateAndEndDateOfDentist(start, end, userId);
        }

        [HttpGet("dentist/appointments")]
        public async Task<IActionResult> GetAppointmentByDentistId([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            string? userIdString = JwtProvider.GetUserId(Request);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("Invalid user ID.");
            }
            var response = await _appointmentService.GetAppointmentByDentistId(page, limit, userId);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("appointmentdetails/{id}")]
        public async Task<IActionResult> GetAppointmentetail(Guid id, [FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _appointmentService.GetAppointmentetail(id, page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("viewappointmentdetails/{id}")]
        public async Task<IActionResult> ViewAppointmentDetail(Guid id)
        {
            var response = await _appointmentService.ViewAppointmentDetail(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("reschedule/{id}")]
        public async Task<IActionResult> RescheduleAppointment(Guid id, AppointmentDetailsDto appmnt)
        {
            var response = await _appointmentService.RescheduleAppointment(id, appmnt);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("availableslot")]
        public async Task<IActionResult> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId)
        {
            var response = await _appointmentService.GetDentistAvailableTimeSlots(startDate, dentId);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("rescheduletempdent")]
        public async Task<IActionResult> GetRescheduleTempDent(DateTime startDate, int timeSlot, int treatId)
        {
            var response = await _appointmentService.GetRescheduleTempDent(startDate, timeSlot, treatId);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("rescheduletempdentversion02")]
        public async Task<IActionResult> GetAnotherDentist(Guid appointmentId, Guid currentDentistId, DateTime startDate, int timeSlot, int treatId)
        {
            var response = await _appointmentService.GetAnotherDentist(appointmentId, currentDentistId, startDate, timeSlot, treatId);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        //[HttpPost("sendConfirmEmail")]
        //public async Task<IActionResult> SendEmail(Guid id)
        //{
        //    await _appointmentService.SendEmailWithConfirmationLink(id);
        //    return Ok();
        //}

        [HttpPost("generateconfirmtoken")]
        public async Task<IActionResult> GenerateConfirmEmailToken(AppointmentDetailsDto appmnt)
        {
            var response = await _appointmentService.GenerateConfirmEmailToken(appmnt);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        //[HttpGet("reschedule/{token}")]
        //public IActionResult ConfirmReschedule(string token)
        //{
        //    try
        //    {
        //        var claims = _tokenService.ValidateToken(token);
        //        var appointmentId = claims.FirstOrDefault(c => c.Type == "AppointmentId")?.Value;
        //        var DentId = claims.FirstOrDefault(c => c.Type == "DentId")?.Value;
        //        var tempDentId = claims.FirstOrDefault(c => c.Type == "TempDentId")?.Value;
        //        var cusId = claims.FirstOrDefault(c => c.Type == "CusId")?.Value;

        //        // Implement your logic here. For example, mark the appointment as confirmed in the database.

        //        return Ok(new { Message = "Appointment reschedule confirmed successfully", AppointmentId = appointmentId, TempDentId = tempDentId, CusId = cusId });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = "Invalid token", Error = ex.Message });
        //    }
        //}

        [HttpPut("reschedulebydentist/{id}")]
        public async Task<IActionResult> RescheduleByDentist(AppointmentDetailsDto appmnt)
        {
            var response = await _appointmentService.RescheduleByDentist(appmnt);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("updateappointmentnote/{id}")]
        public async Task<IActionResult> UpdatAppointmenteNote(Guid id, AppointmentDetailsDto appmnt)
        {
            id = appmnt.Id;
            var response = await _appointmentService.UpdatAppointmenteNote(appmnt);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}
