using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;
using System.Collections.Generic;

namespace swp391_debo_be.Cores
{
    public class CAppointment
    {
        protected static IAppointmentRepository appointmentRepository = new AppointmentRepository();
        private static readonly AppointmentRepository _appointmentRepo;

        static CAppointment()
        {
            var context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());
            _appointmentRepo = new AppointmentRepository(new AppointmentDao(context));
        }


        public static List<object> GetAppointmentsByStartDateAndEndDate(DateTime startDate, DateTime end ,Guid Id)
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
        
        public static int[][] GetApppointmentsByDentistIdAndDate(Guid dentistId, DateTime date, int treatmentId)
        {
            try
            {
                return appointmentRepository.GetApppointmentsByDentistIdAndDate(dentistId, date, treatmentId);
            } catch
            {
                throw;
            }
        }

        public static List<Appointment> CreateAppointment(AppointmentDto dto, Guid cusId)
        {
            try
            {
                return appointmentRepository.CreateAppointment(dto, cusId);
            } catch
            {
                throw;
            }
        }

        public static Appointment CancelAppointment(Guid appointmentId)
        {
            try
            {
                return appointmentRepository.CancelAppointment(appointmentId);
            }
            catch
            {
                throw;
            }
        }
        public static Task<List<AppointmentHistoryDto>> GetHistoryAppointmentByUserID(Guid id)
        {
                return appointmentRepository.GetHistoryAppointmentByUserID(id);
        }

        public static Task<List<AppointmentHistoryDto>> ViewAllAppointment(int page, int limit)
        {
            return appointmentRepository.ViewAllAppointment(page, limit);
        }

        public static List<object> GetAppointmentsByStartDateAndEndDateOfDentist(DateTime startDate, DateTime endDate, Guid Id)
        {
            try
            {
                return appointmentRepository.GetAppointmentsByStartDateAndEndDateOfDentist(startDate, endDate, Id);
            }
            catch
            {
                throw;
            }
        }

        public static Task<List<AppointmentHistoryDto>> GetAppointmentByDentistId(int page, int limit, Guid dentistId)
        {
            return appointmentRepository.GetAppointmentByDentistId(page, limit, dentistId);
        }

        public static Task<List<AppointmentDetailsDto>> GetAppointmentetail(Guid id, int page, int limit)
        {
            return appointmentRepository.GetAppointmentetail(id, page, limit);
        }

        public static Task<AppointmentDetailsDto> ViewAppointmentDetail(Guid id)
        {
            return appointmentRepository.ViewAppointmentDetail(id);
        }
        public static Task RescheduleAppointment(Guid id, AppointmentDetailsDto appmnt)
        {
            return appointmentRepository.RescheduleAppointment(id, appmnt);
        }

        public static Task<List<AppointmentDto>> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId)
        {
            return appointmentRepository.GetDentistAvailableTimeSlots(startDate, dentId);
        }

        public static Task<List<AppointmentDetailsDto>> GetRescheduleTempDent(DateTime startDate, int timeSlot, int treatId)
        {
            return appointmentRepository.GetRescheduleTempDent(startDate, timeSlot, treatId);
        }

        public static Task<List<AppointmentDetailsDto>> GetAnotherDentist(Guid appointmentId, Guid currentDentistId, DateTime startDate, int timeSlot, int treatId)
        {
            return appointmentRepository.GetAnotherDentist(appointmentId, currentDentistId, startDate, timeSlot, treatId);
        }

        public static Task RescheduleByDentist(AppointmentDetailsDto appmnt)
        {
            return appointmentRepository.RescheduleByDentist(appmnt);
        }

        public static Task UpdatAppointmenteNote(AppointmentDetailsDto appmnt)
        {
            return appointmentRepository.UpdatAppointmenteNote(appmnt);
        }

        public static Task SaveRescheduleToken(Guid appmtId, string rescheduleToken)
        {
            return appointmentRepository.SaveRescheduleToken(appmtId, rescheduleToken);
        }
    }
}
