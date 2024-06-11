using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IAppointmentRepository
    {
        public bool CreateAppointment(AppointmentDto appointment);
        public object GetAppointmentByPagination(string page, string limit, Guid userId);
        public List<object> GetAppointmentsByStartDateAndEndDate(DateOnly startDate,DateOnly endDate ,Guid id);
        public List<int> GetApppointmentsByDentistIdAndDate(Guid dentistId, DateOnly date);
        public Appointment CancelAppointment(Guid appointmentId);
        public Task<List<AppointmentHistoryDto>> GetHistoryAppointmentByUserID(Guid id);
        public Task<List<AppointmentDto>> ViewAllAppointment(int page, int limit);
        public List<object> GetAppointmentsByStartDateAndEndDateOfDentist(DateOnly startDate, DateOnly endDate, Guid id);
    }
}
