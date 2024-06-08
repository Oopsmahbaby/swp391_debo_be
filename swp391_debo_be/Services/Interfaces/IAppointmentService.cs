using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IAppointmentService
    {
        ActionResult<ApiRespone> CreateAppointment(AppointmentDto dto, object result);
        public ApiRespone GetAppointmentByPagination(string page, string limit, string userId);
        public ApiRespone GetAppointmentsByStartDateAndEndDate(string startDate,string endDate ,string userId);
        public ApiRespone GetApppointmentsByDentistIdAndDate(string dentistId, string date);
    }
}
