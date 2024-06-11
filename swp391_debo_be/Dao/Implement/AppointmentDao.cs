using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Helpers;

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

        public bool CreateAppointment(AppointmentDto dto)
        {
            Appointment addedAppointment = new Appointment
            {
                Id = Guid.NewGuid(),
                CusId = dto.CusId,
                CreatorId = dto.CreatorId,
                DentId = dto.DentId,
                TreatId = dto.TreatId,
                StartDate = dto.StartDate,
                TimeSlot = dto.TimeSlot,
                Status = "pending",
                Description = dto.Description,
                Note = dto.Note,
                IsCreatedByStaff = dto.IsCreatedByStaff,
                CreatedDate = dto.CreatedDate
            };

            _context.Appointments.Add(addedAppointment);
            _context.SaveChanges();
            return true;
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
                    Id = appointment.Id,
                    Start = appointment.StartDate.HasValue ? (DateOnly)appointment.StartDate : default(DateOnly),
                    TimeSlot = appointment.TimeSlot,
                    Name = treatment?.Result.Name
                });
            }

            int totalCount = _context.Appointments.Count(a => a.CusId == userId);

            return new
            {
                Count = totalCount,
                Appointments = result
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
                .Where(a => a.DentId == dentistId && a.StartDate == date)
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
                                          TreatmentName = ct.Name,
                                          CreatedDate = a.CreatedDate,
                                          StartDate = a.StartDate
                                      }).ToListAsync();
            return appointments;
        }

        public async Task<List<AppointmentDto>> ViewAllAppointment(int page, int limit)
        {
            IQueryable<AppointmentDto> query = from a in _context.Appointments
                                               join ct in _context.ClinicTreatments on a.TreatId equals ct.Id
                                               select new AppointmentDto
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
    }
}
