using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Cores
{
    public class CAppointment
    {
        protected static IAppointmentRepository appointmentRepository = new AppointmentRepository();

        public static List<object> GetAppointmentsByStartDateAndEndDate(DateOnly startDate,DateOnly end ,Guid Id)
        {
            try
            {
                return appointmentRepository.GetAppointmentsByStartDateAndEndDate(startDate, end ,Id);
            } catch 
            {
                throw;
            }
        }

        public static object GetAppointmentByPagination(string page, string limit, Guid userId)
        {
            try
            {
                return appointmentRepository.GetAppointmentByPagination(page, limit, userId);
            } catch
            {
                throw;
            }
        }
        
        public static List<int> GetApppointmentsByDentistIdAndDate(Guid dentistId, DateOnly date)
        {
            try
            {
                return appointmentRepository.GetApppointmentsByDentistIdAndDate(dentistId, date);
            } catch
            {
                throw;
            }
        }
    }
}
