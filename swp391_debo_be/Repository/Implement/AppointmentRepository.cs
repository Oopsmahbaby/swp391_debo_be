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
        //private readonly IAppointmentDao _appointmentDao;

        //public AppointmentRepository()
        //{
        //}

        //public AppointmentRepository(IAppointmentDao appointmentDao)
        //{
        //    _appointmentDao = appointmentDao;
        //}

        public List<Appointment> CreateAppointment(AppointmentDto appointment, Guid cusId)
        {
            return appointmentDao.CreateAppointment(appointment, cusId);
        }

        public object GetAppointmentByPagination(string page, string limit, Guid userId)
        {
            return appointmentDao.GetAppointmentByPagination(page, limit, userId);
        }

        public List<object> GetAppointmentsByStartDateAndEndDate(DateTime startDate, DateTime endDate, Guid Id)
        {
           return appointmentDao.GetAppointmentsByStartDateAndEndDate(startDate, endDate ,Id);
        }

        public int[][] GetApppointmentsByDentistIdAndDate(Guid dentistId, DateTime date, int treatmentId)
        {
            return appointmentDao.GetApppointmentsByDentistIdAndDate(dentistId, date, treatmentId);
        }

        public Appointment CancelAppointment(Guid appointmentId)
        {
            return appointmentDao.CancelAppointment(appointmentId);
        }
        public Task<List<AppointmentHistoryDto>> GetHistoryAppointmentByUserID(Guid id)
        {
            return appointmentDao.GetHistoryAppointmentByUserID (id);
        }

        public Task<List<AppointmentHistoryDto>> ViewAllAppointment(int page, int limit)
        {
            return appointmentDao.ViewAllAppointment(page, limit);
        }

        public List<object> GetAppointmentsByStartDateAndEndDateOfDentist(DateTime startDate, DateTime endDate, Guid id)
        {
            return appointmentDao.GetAppointmentsByStartDateAndEndDateOfDentist (startDate, endDate, id);
        }

        public Task<List<AppointmentHistoryDto>> GetAppointmentByDentistId(int page, int limit, Guid dentistId)
        {
            return appointmentDao.GetAppointmentByDentistId (page, limit, dentistId);
        }

        public Task<List<AppointmentDetailsDto>> GetAppointmentetail(Guid id, int page, int limit)
        {
            return appointmentDao.GetAppointmentetail (id, page, limit);
        }

        public Task<AppointmentDetailsDto> ViewAppointmentDetail(Guid id)
        {
            return appointmentDao.ViewAppointmentDetail (id);
        }

        public Task RescheduleAppointment(Guid id, AppointmentDetailsDto appmnt)
        {
            return appointmentDao.RescheduleAppointment (id, appmnt);
        }

        public Task<List<AppointmentDto>> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId)
        {
            return appointmentDao.GetDentistAvailableTimeSlots (startDate, dentId);
        }

        public Task<List<AppointmentDetailsDto>> GetRescheduleTempDent(DateTime startDate, int timeSlot, int treatId)
        {
            return appointmentDao.GetRescheduleTempDent (startDate, timeSlot, treatId);
        }

        public Task RescheduleByDentist(AppointmentDetailsDto appmnt)
        {
            return appointmentDao.RescheduleByDentist (appmnt);
        }

        public Task UpdatAppointmenteNote(AppointmentDetailsDto appmnt)
        {
            return appointmentDao.UpdatAppointmenteNote (appmnt);
        }

        public Task SaveRescheduleToken(Guid appmtId, string rescheduleToken)
        {
            return appointmentDao.SaveRescheduleToken (appmtId, rescheduleToken);
        }

        public Task<List<AppointmentDetailsDto>> GetAnotherDentist(Guid appointmentId, Guid currentDentistId, DateTime startDate, int timeSlot, int treatId)
        {
            return appointmentDao.GetAnotherDentist(appointmentId, currentDentistId, startDate, timeSlot, treatId);
        }
    }
}
