using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Services.Implements
{
    public class AppointmentService : IAppointmentService
    {
        public ApiRespone GetAppointmentByPagination(string page, string limit, string userId)
        {
            try
            {
                if (Guid.TryParse(userId, out Guid Id))
                {
                    var result = CAppointment.GetAppointmentByPagination(page, limit, Id);

                    if (result == null)
                    {
                        return new ApiRespone {StatusCode = System.Net.HttpStatusCode.NotFound, Data = null, Message = "No appointment found", Success = false };
                    }

                    return new ApiRespone {StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched appointment list successfully", Success = true };
                } else
                {
                       return new ApiRespone {StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid user id", Success = false };
                }

            } catch (Exception ex)
            {
                return new ApiRespone {StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public ApiRespone GetAppointmentsByStartDateAndEndDate(string startDate, string userId)
        {
            try
            {
                if (DateOnly.TryParse(startDate, out DateOnly start) && Guid.TryParse(userId, out Guid Id))
                {

                    ActionResult<List<object>> appointments = CAppointment.GetAppointmentsByStartDateAndEndDate(start, Id);

            
                    if (appointments.Value.Count == 0)
                    {
                        return new ApiRespone {StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "No appointment found", Success = false };
                    }

                    return new ApiRespone {StatusCode = System.Net.HttpStatusCode.OK, Data = appointments, Message = "Get appointments successfully", Success = true };
                }
                else
                {
                    return new ApiRespone {StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = "Invalid date format", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
    }
}
