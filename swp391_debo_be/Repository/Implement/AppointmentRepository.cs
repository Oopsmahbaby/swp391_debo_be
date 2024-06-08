using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Repository.Implement
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IAppointmentDao appointmentDao = new AppointmentDao();
        private readonly IAppointmentDao _appointmentDao;

        public AppointmentRepository()
        {
        }

        public AppointmentRepository(IAppointmentDao appointmentDao)
        {
            _appointmentDao = appointmentDao;
        }

        public object GetAppointmentByPagination(string page, string limit, Guid userId)
        {
            return appointmentDao.GetAppointmentByPagination(page, limit, userId);
        }

        public List<object> GetAppointmentsByStartDateAndEndDate(DateOnly startDate, DateOnly endDate, Guid Id)
        {
           return appointmentDao.GetAppointmentsByStartDateAndEndDate(startDate, endDate ,Id);
        }

        public Task<List<AppointmentHistoryDto>> GetHistoryAppointmentByUserID(Guid id)
        {
            return _appointmentDao.GetHistoryAppointmentByUserID (id);
        }
    }
}
