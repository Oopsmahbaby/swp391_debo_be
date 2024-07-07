using Azure.Core;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Globalization;
using System.Net;
using System.Security.Claims;

namespace swp391_debo_be.Services.Implements
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IConfiguration _config;

        public AppointmentService(IConfiguration config)
        {
            _config = config;
        }
        public ApiRespone CancelAppointment(string id)
        {
            try
            {
                if (Guid.TryParse(id, out Guid Id))
                {
                    var result = CAppointment.CancelAppointment(Id);
                    var data = CAppointment.ViewAppointmentDetail(Id).Result;
                    SendCancelEmailToDentist(data).Wait();
                    return result != null ? new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Deleted Appointment successfully", Success = true }
                    : new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Failed to cancel appointment", Success = false };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid appointment id", Success = false };
                }
            }
            catch (System.Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public ApiRespone CreateAppointment(AppointmentDto dto, string userId, string role)
        {
            try
            {
                if (role != "Customer")
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.Unauthorized, Data = null, Message = "You are not allowed to create appointment", Success = false };
                }

                if (Guid.TryParse(userId, out var id) && DateTime.TryParse(dto.Date, out DateTime startDate))
                {

                    var result = CAppointment.CreateAppointment(dto, id);

                    return result != null ? new ApiRespone { StatusCode = System.Net.HttpStatusCode.Created, Data = result, Message = "Created appointment successfully", Success = true }
                    : new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Failed to create appointment", Success = false };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid user id", Success = false };
                }
            }
            catch (System.Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetAppointmentByDentistId(int page, int limit, Guid dentistId)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CAppointment.GetAppointmentByDentistId(page, limit, dentistId);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Data = new { list = data, total = data.Count };
                response.Success = true;
                response.Message = "Fetched appointment list successfully";
                return response;
            }
            catch (System.Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        //public ActionResult<ApiRespone> CreateAppointment(AppointmentDto dto, object result)
        //{
        //    //return //AppointmentDto.CreateAppointment(dto, result);
        //}

        public ApiRespone GetAppointmentByPagination(string page, string limit, string userId)
        {
            try
            {
                if (Guid.TryParse(userId, out Guid Id))
                {
                    var result = CAppointment.GetAppointmentByPagination(page, limit, Id);

                    if (result == null)
                    {
                        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Data = null, Message = "No appointment found", Success = false };
                    }

                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched appointment list successfully", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid user id", Success = false };
                }

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetAppointmentetail(Guid id, int page, int limit)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CAppointment.GetAppointmentetail(id, page, limit);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = new { list = data, total = data.Count };
                response.Success = true;
                response.Message = "Appointment data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ApiRespone GetAppointmentsByStartDateAndEndDate(string startDate, string endDate, string userId)
        {
            try
            {
                if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end) && Guid.TryParse(userId, out Guid Id))
                {

                    ActionResult<List<object>> appointments = CAppointment.GetAppointmentsByStartDateAndEndDate(start, end, Id);


                    if (appointments.Value.Count == 0)
                    {
                        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "No appointment found", Success = false };
                    }

                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = appointments, Message = "Get appointments successfully", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid date format", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public ApiRespone GetAppointmentsByStartDateAndEndDateOfDentist(string startDate, string endDate, string userId)
        {
            try
            {
                if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end) && Guid.TryParse(userId, out Guid Id))
                {

                    ActionResult<List<object>> appointments = CAppointment.GetAppointmentsByStartDateAndEndDateOfDentist(start, end, Id);


                    if (appointments.Value.Count == 0)
                    {
                        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "No appointment found", Success = false };
                    }

                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = appointments, Message = "Get appointments successfully", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid date format", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        //public ApiRespone GetApppointmentsByDentistIdAndDate(string dentistId, string date, string treatmentId)
        //{
        //    try
        //    {
        //        if (Guid.TryParse(dentistId, out Guid dentist) && DateTime.TryParse(date, out DateTime dateOnly) && int.TryParse(treatmentId, out int treatId))
        //        {
        //            var result = CAppointment.GetApppointmentsByDentistIdAndDate(dentist, dateOnly,treatId);

        //            if (result == null)
        //            {
        //                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Data = null, Message = "No slots found", Success = false };
        //            }

        //            return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched slots successfully.", Success = true };
        //        }
        //        else
        //        {
        //            return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid dentist id or date", Success = false };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
        //    }
        //}

        public async Task<ApiRespone> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId)
        {
            try
            {
                var data = await CAppointment.GetDentistAvailableTimeSlots(startDate, dentId);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Fetched slots successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetHistoryAppointmentByUserID(Guid userId)
        {
            try
            {
                var data = await CAppointment.GetHistoryAppointmentByUserID(userId);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Treatment data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> RescheduleAppointment(Guid id, AppointmentDetailsDto appmnt)
        {
            var response = new ApiRespone();
            try
            {
                await CAppointment.RescheduleAppointment(id, appmnt);
                var data = await CAppointment.ViewAppointmentDetail(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Rescheduled successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewAllAppointment(int page, int limit)
        {
            try
            {
                var data = await CAppointment.ViewAllAppointment(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Appointment data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewAppointmentDetail(Guid id)
        {
            try
            {
                var data = await CAppointment.ViewAppointmentDetail(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Appointment data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
        public ApiRespone GetApppointmentsByDentistIdAndDate(string dentistId, string date, string treatmentId)
        {
            try
            {
                if (Guid.TryParse(dentistId, out Guid dentist) && DateTime.TryParse(date, CultureInfo.InvariantCulture, out DateTime dateOnly) && int.TryParse(treatmentId, out int treatId))
                {
                    var result = CAppointment.GetApppointmentsByDentistIdAndDate(dentist, dateOnly, treatId);

                    if (result == null) // Assuming result is a collection of appointments
                    {
                        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Data = null, Message = "No slots found", Success = false };
                    }

                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched slots successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid dentist id, date, or treatment id format", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.InternalServerError, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetRescheduleTempDent(DateTime startDate, int timeSlot, int treatId)
        {
            try
            {
                var data = await CAppointment.GetRescheduleTempDent(startDate, timeSlot, treatId);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Appointment data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task SendEmailWithConfirmationLink(Guid id, string confirmationLink)
        {
            try
            {
                var user = await CUser.GetUserById2(id);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = $"[DEBO] Dear {user.FirstName} {user.LastName}, Appointment Reschedule Confirmation";
                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
        }}
        .container {{
            width: 100%;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .header {{
            background-color: #4CAF50;
            color: white;
            padding: 10px 0;
            text-align: center;
            font-size: 24px;
        }}
        .content {{
            margin: 20px 0;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            color: white;
            background-color: #4CAF50;
            text-align: center;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Appointment Reschedule Confirmation
        </div>
        <div class='content'>
            <p>Hi {user.FirstName} {user.LastName},</p>
            <p>We have sent a request to reschedule your appointment with a new dentist. Please click the button below to confirm this change:</p>
            <p><a href='{confirmationLink}' class='button'>Confirm Appointment Reschedule</a></p>
            <p>If you did not request this change, please ignore this email or contact us immediately.</p>
            <p>Thank you for your attention!</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                smtp.Connect(
                    _config.GetSection("EmailHost").Value,
                    int.Parse(_config.GetSection("EmailPort").Value!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                smtp.Authenticate(
                    _config.GetSection("EmailUsername").Value,
                    _config.GetSection("EmailPassword").Value
                );
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email to customer: {ex.Message}");
                throw; // Rethrow the exception to handle it at a higher level if needed
            }
        }

        public async Task SendConfirmEmailToDentist(AppointmentDetailsDto appmnt)
        {
            try
            {
                var dentist = await CUser.GetUserById2((Guid)appmnt.Dent_Id!);
                var tempdent = await CUser.GetUserById2((Guid)appmnt.Temp_Dent_Id!);
                var user = await CUser.GetUserById2((Guid)appmnt.Cus_Id!);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(dentist.Email));
                email.Subject = $"[DEBO] Dear {dentist.FirstName} {dentist.LastName} Reschedule Confirmation";
                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            width: 100%;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .header {{
            background-color: #4CAF50;
            color: white;
            padding: 10px 0;
            text-align: center;
            font-size: 24px;
        }}
        .content {{
            margin: 20px 0;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Appointment Confirmation
        </div>
        <div class='content'>
            <p>Dear Dr. {dentist.FirstName + " " + dentist.LastName},</p>
            <p>We are pleased to inform you that a customer has confirmed their appointment.</p>
            <p><strong>Customer Name:</strong> {user.FirstName + " " + user.LastName}</p>
            <p><strong>Appointment Date:</strong> {appmnt.StartDate}</p>
            <p><strong>Temporary Dentist:</strong> {tempdent.FirstName + " " + tempdent.LastName}</p>
            <p>Thank you!</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
            <p>&copy; 2024 Your Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                smtp.Connect(
                    _config.GetSection("EmailHost").Value,
                    int.Parse(_config.GetSection("EmailPort").Value!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                smtp.Authenticate(
                    _config.GetSection("EmailUsername").Value,
                    _config.GetSection("EmailPassword").Value
                );
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email to dentist: {ex.Message}");
                throw; // Rethrow the exception to handle it at a higher level if needed
            }
        }

        public async Task SendConfirmEmailToTempDentist(AppointmentDetailsDto appmnt)
        {
            try
            {
                var dentist = await CUser.GetUserById2((Guid)appmnt.Dent_Id!);
                var tempdent = await CUser.GetUserById2((Guid)appmnt.Temp_Dent_Id!);
                var user = await CUser.GetUserById2((Guid)appmnt.Cus_Id!);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(tempdent.Email));
                email.Subject = $"[DEBO] Dear {tempdent.FirstName} {tempdent.LastName} Appointment Assignment";
                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            width: 100%;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .header {{
            background-color: #4CAF50;
            color: white;
            padding: 10px 0;
            text-align: center;
            font-size: 24px;
        }}
        .content {{
            margin: 20px 0;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Appointment Assignment
        </div>
        <div class='content'>
            <p>Dear Dr. {tempdent.FirstName} {tempdent.LastName},</p>
            <p>We are pleased to inform you that you have been assigned to the following appointment:</p>
            <p><strong>Customer Name:</strong> {user.FirstName} {user.LastName}</p>
            <p><strong>Appointment Date:</strong> {appmnt.StartDate}</p>
            <p>Please be prepared for the appointment and ensure all necessary arrangements are in place.</p>
            <p>Thank you!</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                smtp.Connect(
                    _config.GetSection("EmailHost").Value,
                    int.Parse(_config.GetSection("EmailPort").Value!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                smtp.Authenticate(
                    _config.GetSection("EmailUsername").Value,
                    _config.GetSection("EmailPassword").Value
                );
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email to temporary dentist: {ex.Message}");
                throw; // Rethrow the exception to handle it at a higher level if needed
            }
        }

        public async Task SendCancelEmailToDentist(AppointmentDetailsDto appmnt)
        {
            try
            {
                var dentist = await CUser.GetUserById2((Guid)appmnt.Dent_Id!);
                var tempdent = appmnt.Temp_Dent_Id.HasValue ? await CUser.GetUserById2((Guid)appmnt.Temp_Dent_Id) : null;
                var user = await CUser.GetUserById2((Guid)appmnt.Cus_Id!);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                if (appmnt.Temp_Dent_Id != null)
                {
                    email.To.Add(MailboxAddress.Parse(tempdent!.Email));
                }
                else
                {
                    email.To.Add(MailboxAddress.Parse(dentist.Email));
                }
                string salutation = tempdent != null
            ? $"Dear Dr. {tempdent.FirstName} {tempdent.LastName},"
            : $"Dear Dr. {dentist.FirstName} {dentist.LastName},";
                email.Subject = $"[DEBO] {salutation} Appointment Cancellation";
                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            width: 100%;
            padding: 20px;
            background-color: #f9f9f9;
        }}
        .header {{
            background-color: red;
            color: white;
            padding: 10px 0;
            text-align: center;
            font-size: 24px;
        }}
        .content {{
            margin: 20px 0;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #888;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Appointment Cancel
        </div>
        <div class='content'>
            <p>{salutation}</p>
            <p>We regret to inform you that your patient, {user.FirstName} {user.LastName}, has canceled their appointment scheduled on {appmnt.StartDate}.</p>
            <p>If you have any questions or need further assistance, please feel free to contact us.</p>
            <p>Thank you for your understanding.</p>
            <br />
            <p>Sincerely,</p>
            <p>DEBO Clinic</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
            <p>&copy; 2024 Your Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using var smtp = new SmtpClient();
                smtp.Connect(
                    _config.GetSection("EmailHost").Value,
                    int.Parse(_config.GetSection("EmailPort").Value!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );
                smtp.Authenticate(
                    _config.GetSection("EmailUsername").Value,
                    _config.GetSection("EmailPassword").Value
                );
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending email to dentist: {ex.Message}");
                throw; // Rethrow the exception to handle it at a higher level if needed
            }
        }

        public async Task<ApiRespone> GenerateConfirmEmailToken(AppointmentDetailsDto appmnt)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim("AppointmentId", appmnt.Id.ToString()),
                    new Claim("DentId", appmnt.Dent_Id?.ToString() ?? string.Empty),
                    new Claim("TempDentId", appmnt.Temp_Dent_Id?.ToString() ?? string.Empty),
                    new Claim("CusId", appmnt.Cus_Id.ToString() ?? string.Empty)
                };

                string confirmToken = JwtProvider.GenerateToken(claims);
                await CAppointment.SaveRescheduleToken(appmnt.Id, confirmToken);
                string confirmationLink = $"http://localhost:5173/patient/reschedule/{confirmToken}";
                await SendEmailWithConfirmationLink((Guid)appmnt.Cus_Id!, confirmationLink);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = confirmToken, Success = true, Message = "Generate token successfully" };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> RescheduleByDentist(AppointmentDetailsDto appmnt)
        {
            try
            {
                await CAppointment.RescheduleByDentist(appmnt);
                var data = await CAppointment.ViewAppointmentDetail(appmnt.Id);
                await SendConfirmEmailToDentist(appmnt);
                await SendConfirmEmailToTempDentist(appmnt);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Rescheduled successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> UpdatAppointmenteNote(AppointmentDetailsDto appmnt)
        {
            try
            {
                await CAppointment.UpdatAppointmenteNote(appmnt);
                var data = await CAppointment.ViewAppointmentDetail(appmnt.Id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Update Note successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetAnotherDentist(Guid appointmentId, Guid currentDentistId, DateTime startDate, int timeSlot, int treatId)
        {
            try
            {
                var data = await CAppointment.GetAnotherDentist(appointmentId, currentDentistId, startDate, timeSlot, treatId);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Retrieve Temporary Dentist data successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
    }
}
