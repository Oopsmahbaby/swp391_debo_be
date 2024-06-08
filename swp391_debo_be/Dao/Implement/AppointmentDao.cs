using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public bool CreateAppointment(AppointmentDto dto, Guid userId)
        {
            Appointment addedAppointment = new Appointment
            {
                Id = Guid.NewGuid(),
                CusId = userId,
                CreatorId = userId,
                DentId = dto.DentId,
                TreatId = dto.TreatId,
                StartDate = dto.StartDate,
                TimeSlot = dto.TimeSlot,
                Status = "pending",
                Description = dto.Description,
                Note = dto.Note,
                IsCreatedByStaff = false
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
                var treatment =  _context.ClinicTreatments
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
    }
}
