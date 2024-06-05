using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IAppointmentRepository
    {
        public object GetAppointmentByPagination(string page, string limit, Guid userId);
        public List<object> GetAppointmentsByStartDateAndEndDate(DateOnly startDate,DateOnly endDate ,Guid id);
    }
}
