using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IAppointmentService
    {
        public ApiRespone GetAppointmentByPagination(string page, string limit, string userId);
        public ApiRespone GetAppointmentsByStartDateAndEndDate(string startDate,string endDate ,string userId);
        public Task<ApiRespone> GetHistoryAppointmentByUserID(Guid userId);
    }
}
