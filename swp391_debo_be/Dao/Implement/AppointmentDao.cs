﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Logging;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace swp391_debo_be.Dao.Implement
{
    public class AppointmentDao : IAppointmentDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context(new Microsoft.EntityFrameworkCore.DbContextOptions<DeboDev02Context>());
        public AppointmentDao() { }

        public AppointmentDao(DeboDev02Context context)
        {
            _context = context;
        }

        public Appointment CancelAppointment(Guid appointmentId)
        {
            Appointment appointment = _context.Appointments
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();

            if (appointment == null)
            {
                return null;
            }

            appointment.Status = "cancelled";
            _context.Update(appointment);
            _context.SaveChanges();

            return appointment;
        }

        public Appointment CreateAppointment(Appointment dto)
        {   
            _context.Appointments.Add(dto);
            _context.SaveChanges();
            return dto;
        }

        public object GetAppointmentByPagination(string page, string limit, Guid userId)
        {
            int pageNumber = int.Parse(page);
            int pageSize = int.Parse(limit);
            int skip = pageNumber * pageSize;

            var appointments = new List<Appointment>();
            if (pageSize == -1)
            {
                appointments = _context.Appointments
                   .Where(a => a.CusId == userId)
                   .ToList();
            }
            else
            {
                appointments = _context.Appointments
                    .Where(a => a.CusId == userId)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
            }


            List<object> result = new List<object>();

            foreach (Appointment appointment in appointments)
            {
                var treatment = _context.ClinicTreatments
                    .Where(t => t.Id == appointment.TreatId)
                    .FirstOrDefaultAsync();

                result.Add(new
                {
                    id = appointment.Id,
                    start = appointment.StartDate.HasValue ? (DateOnly)appointment.StartDate : default(DateOnly),
                    timeSlot = appointment.TimeSlot,
                    name = treatment?.Result.Name,
                    dentist = _context.Users.Where(u => u.Id == appointment.DentId).FirstOrDefault().FirstName + " " + _context.Users.Where(u => u.Id == appointment.DentId).FirstOrDefault().LastName,
                    treatment = treatment?.Result.Name,
                    status = appointment.Status
                }); ;
            }

            int totalCount = _context.Appointments.Count(a => a.CusId == userId);

            return new
            {
                Total = totalCount,
                List = result
            };
        }

        public List<object> GetAppointmentsByStartDateAndEndDate(DateOnly startDate, DateOnly endDate, Guid Id)
        {
            var appointments = _context.Appointments.Where(a => a.StartDate >= startDate && a.StartDate <= endDate && Guid.Equals(a.CusId, Id)).ToList();


            if (appointments == null)
            {
                return new List<object>();
            }

            List<object> result = new List<object>();

            foreach (Appointment appointment in appointments)
            {
                var treatmeant = _context.ClinicTreatments.Where(t => t.Id == appointment.TreatId).FirstOrDefault();
                result.Add(new { Id = appointment.Id, start = (DateOnly)appointment.StartDate, TimeSlot = appointment.TimeSlot, name = treatmeant?.Name });
            }

            return result;
        }

        public List<int> GetApppointmentsByDentistIdAndDate(Guid dentistId, DateOnly date)
        {
            List<int> nonAvailableSlots = new List<int>();

            var appointments = _context.Appointments
                .Where(a => Guid.Equals(a.DentId, dentistId) && a.StartDate == date)
                .ToList();

            foreach (Appointment appointment in appointments)
            {
                nonAvailableSlots.Add((int)appointment.TimeSlot);
            }

            List<int> availableSlots = Slot.GetSlots(nonAvailableSlots);

            return availableSlots;
        }
        public async Task<List<AppointmentHistoryDto>> GetHistoryAppointmentByUserID(Guid id)
        {
            var appointments = await (from a in _context.Appointments
                                      join ct in _context.ClinicTreatments on a.TreatId equals ct.Id
                                      where a.CusId == id
                                      select new AppointmentHistoryDto
                                      {
                                          TreatName = ct.Name,
                                          CreatedDate = a.CreatedDate,
                                          StartDate = a.StartDate
                                      }).ToListAsync();
            return appointments;
        }

        public async Task<List<AppointmentHistoryDto>> ViewAllAppointment(int page, int limit)
        {
            IQueryable<AppointmentHistoryDto> query = from a in _context.Appointments
                                               join ct in _context.ClinicTreatments on a.TreatId equals ct.Id
                                               select new AppointmentHistoryDto
                                               {
                                                   Id = a.Id,
                                                   TreatName = ct.Name,
                                                   PaymentId = (Guid)a.PaymentId,
                                                   DentId = a.DentId,
                                                   TempDentId = a.TempDentId,
                                                   CusId = a.CusId,
                                                   CreatorId = a.CreatorId,
                                                   IsCreatedByStaff = a.IsCreatedByStaff,
                                                   CreatedDate = a.CreatedDate,
                                                   StartDate = a.StartDate,
                                                   TimeSlot = a.TimeSlot,
                                                   Status = a.Status,
                                                   Description = a.Description,
                                                   Note = a.Note,
                                               };

            if (limit > 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }

            var appointments = await query.ToListAsync();
            return appointments;
        }

        public List<object> GetAppointmentsByStartDateAndEndDateOfDentist(DateOnly startDate, DateOnly endDate, Guid Id)
        {
            var appointments = _context.Appointments
                        .Where(a => a.StartDate >= startDate && a.StartDate <= endDate &&
                                    (a.TempDentId == Id || a.DentId == Id) &&
                                    a.Status != "pending" && a.Status != "canceled").ToList();
            if (appointments == null || appointments.Count == 0)
            {
                return new List<object>();
            }

            List<object> result = new List<object>();

            foreach (Appointment appointment in appointments)
            {
                var treatmeant = _context.ClinicTreatments.Where(t => t.Id == appointment.TreatId).FirstOrDefault();
                result.Add(new 
                { 
                    Id = appointment.Id, 
                    start = (DateOnly)appointment.StartDate, 
                    TimeSlot = appointment.TimeSlot, 
                    name = treatmeant?.Name 
                });
            }

            return result;
        }

        public async Task<List<AppointmentHistoryDto>> GetAppointmentByDentistId(int page, int limit, Guid dentistId)
        {
            var query = from a in _context.Appointments
                                      join ct in _context.ClinicTreatments on a.TreatId equals ct.Id
                                      where (a.TempDentId == dentistId || (a.TempDentId == null && a.DentId == dentistId))
                                      select new AppointmentHistoryDto
                                      {
                                          Id = a.Id,
                                          TreatName = ct.Name,
                                          StartDate = a.StartDate,
                                          CusId = a.CusId,
                                          Status = a.Status,
                                      };
            if (limit > 0)
            {
                query = query.Skip(page * limit)
                             .Take(limit);
            }
            var appointments = await query.ToListAsync();
            return appointments;
        }

        public async Task<List<AppointmentDetailsDto>> GetAppointmentetail(Guid id, int page, int limit)
        {
            IQueryable<Appointment> query = _context.Appointments
        .Where(a => a.CusId == id)
        .Include(a => a.Treat)
        .ThenInclude(t => t.CategoryNavigation)
        .Include(a => a.Dent)
        .Include(a => a.TempDent)
        .Include(a => a.Cus)
        .Include(a => a.Creator);

            if (limit > 0)
            {
                query = query.Skip(page * limit)
                             .Take(limit);
            }

            var appointments = await query.ToListAsync();

            var appointmentDetails = appointments.Select(a => new AppointmentDetailsDto
            {
                Id = a.Id,
                CreatedDate = a.CreatedDate ?? default,
                StartDate = a.StartDate,
                TimeSlot = a.TimeSlot,
                Status = a.Status,
                Description = a.Description,
                Note = a.Note,
                CategoryName = a.Treat?.CategoryNavigation?.Name,
                TreatmentName = a.Treat?.Name,
                Price = a.Treat?.Price,
                DentistName = a.TempDentId != null ? a.TempDent?.FirstName + " " + a.TempDent?.LastName : a.Dent?.FirstName + " " + a.Dent?.LastName,
                CustomerName = a.Cus?.FirstName + " " + a.Cus?.LastName,
                CreatorName = a.Creator?.FirstName + " " + a.Creator?.LastName,
                Dent_Avt = a.TempDentId != null ? a.TempDent?.Avt : a.Dent?.Avt
            }).ToList();

            return appointmentDetails;
        }

        public async Task<AppointmentDetailsDto> ViewAppointmentDetail(Guid id)
        {
            var appointment = await _context.Appointments
                .Where(a => a.Id == id)
                .Include(a => a.Treat)
                .ThenInclude(t => t.CategoryNavigation)
                .Include(a => a.Dent)
                .Include(a => a.TempDent)
                .Include(a => a.Cus)
                .Include(a => a.Creator)
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return null;
            }

            var appointmentDetails = new AppointmentDetailsDto
            {
                Id = appointment.Id,
                CreatedDate = appointment.CreatedDate ?? default,
                StartDate = appointment.StartDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                Description = appointment.Description,
                Note = appointment.Note,
                CategoryName = appointment.Treat?.CategoryNavigation?.Name,
                TreatmentName = appointment.Treat?.Name,
                Price = appointment.Treat?.Price,
                DentistName = appointment.TempDentId != null ? appointment.TempDent?.FirstName + " " + appointment.TempDent?.LastName : appointment.Dent?.FirstName + " " + appointment.Dent?.LastName,
                CustomerName = appointment.Cus?.FirstName + " " + appointment.Cus?.LastName,
                CreatorName = appointment.Creator?.FirstName + " " + appointment.Creator?.LastName,
                Dent_Avt = appointment.TempDentId != null ? appointment.TempDent?.Avt : appointment.Dent?.Avt
            };

            return appointmentDetails;
        }
    }
}
