using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
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

        public List<Appointment> CreateAppointment(AppointmentDto dto, Guid cusId)
        {
            int? ruleId = _context.ClinicTreatments
                .Where(t => t.Id == dto.TreateId)
                .Select(t => t.RuleId)
                .FirstOrDefault();

            int? numOfApp = _context.ClinicTreatments
                .Where(t => t.Id == dto.TreateId)
                .Select(t => t.NumOfApp)
                .FirstOrDefault();

            List<DateTime> futureDates = GetFutureDate(DateTime.Parse(dto.Date!), (int)numOfApp!, (int)ruleId!);

            // List to view created appoinment will be deleted when it done
            List<Appointment> createdAppointments = new List<Appointment>();

            foreach(DateTime date in futureDates)
            {
                Appointment appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    CusId = cusId,
                    DentId = Guid.Parse(dto.DentId!),
                    TreatId = dto.TreateId,
                    StartDate = date,
                    TimeSlot = dto.TimeSlot,
                    Status = "pending",
                    CreatedDate = DateTime.Now,
                    CreatorId = cusId,
                    IsCreatedByStaff = false
                };
                createdAppointments.Add(appointment);
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
            }

            return createdAppointments;
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
                    start = appointment.StartDate.HasValue ? appointment.StartDate : default(DateTime),
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

        public List<object> GetAppointmentsByStartDateAndEndDate(DateTime startDate, DateTime endDate, Guid Id)
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
                result.Add(new { Id = appointment.Id, start = appointment.StartDate, TimeSlot = appointment.TimeSlot, name = treatmeant?.Name });
            }

            return result;
        }

        private static List<DateTime> GetFutureDate(DateTime date, int numOfApp, int rule)
        {
            List<DateTime> futureDate = [date];
            for (int i = 0; i < numOfApp - 1; i++)
            {
                switch (rule)
                {
                    case 1:
                        date = date.AddDays(1);
                        futureDate.Add(date);
                        break;
                    case 2:
                        date = date.AddDays(7);
                        futureDate.Add(date);
                        break;
                    case 3:
                        date = date.AddMonths(1);
                        futureDate.Add(date);
                        break;
                    case 4:
                        date = date.AddYears(1);
                        futureDate.Add(date);
                        break;
                    case 5:
                        date = date.AddMonths(6);
                        futureDate.Add(date);
                        break;
                }
            }
            return futureDate;
        }

        public int[][] GetApppointmentsByDentistIdAndDate(Guid dentistId, DateTime date, int treatmentId)
        {
            int? rule = _context.Rules.Where(r => r.Id == treatmentId).Select(r => r.Id).FirstOrDefault();
            int? numOfApp = _context.ClinicTreatments.Where(t => t.Id == treatmentId).Select(t => t.NumOfApp).FirstOrDefault();

            List<DateTime> futureDate = GetFutureDate(date, (int)numOfApp, (int)rule);
            int[][] timeSlot = new int[futureDate.Count][];

            for (int i = 0; i < futureDate.Count; i++)
            {
                timeSlot[i] = _context.Appointments
                    .Where(a => a.DentId == dentistId && a.StartDate == futureDate[i] && (a.Status == "pending" || a.Status  == "future"))
                    .Select(a => (int)a.TimeSlot)
                    .ToArray();
            }

            int[][] availableSLots = GetAvailableSlots(timeSlot);
            return availableSLots;
        }

        private static int[][] GetAvailableSlots(int[][] slots)
        {
            int[][] availableSlots = new int[slots.Length][];
            for (int i = 0; i < slots.Length; i++)
            {
                List<int> availableSlot = new List<int>();
                for (int j = 7; j <= 19 ; j++)
                {
                    if (!slots[i].Contains(j))
                    {
                        availableSlot.Add(j);
                    }
                }
                availableSlots[i] = availableSlot.ToArray();
            }
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
                                                   PaymentId = a.PaymentId,
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

        public List<object> GetAppointmentsByStartDateAndEndDateOfDentist(DateTime startDate, DateTime endDate, Guid Id)
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
                    start = appointment.StartDate, 
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

        public async Task RescheduleAppointment(Guid id, AppointmentDetailsDto appmnt)
        {
            // Get the appointment to be rescheduled
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                throw new ArgumentException("Appointment not found.");
            }

            // Check the status of the appointment
            var validStatuses = new List<string> { "pending", "on-going", "future" };
            if (!validStatuses.Contains(appointment.Status!))
            {
                throw new ArgumentException("Only appointments with status 'pending', 'on-going', or 'future' can be rescheduled.");
            }

            // Parse the new start date from the DTO
            if (!appmnt.StartDate.HasValue)
            {
                throw new ArgumentException("Start date is required.");
            }
            DateTime newStartDate = appmnt.StartDate.Value;

            // Ensure the new start date's time slot is valid
            if (!appmnt.TimeSlot.HasValue || appmnt.TimeSlot < 7 || appmnt.TimeSlot > 19)
            {
                throw new ArgumentException("Invalid time slot. Must be between 7 and 19.");
            }

            // Combine the new start date and time slot
            newStartDate = newStartDate.Date.AddHours(appmnt.TimeSlot.Value);

            // Ensure the new start date is greater than or equal to the current time
            DateTime currentTime = DateTime.Now;
            if (newStartDate < currentTime)
            {
                throw new ArgumentException("The new start date and time must be greater than or equal to the current time.");
            }

            // If the new start date equals the current date, calculate the difference between the current hour and the new time slot
            if (newStartDate.Date == currentTime.Date)
            {
                int currentHour = currentTime.Hour;
                if (appmnt.TimeSlot.Value - currentHour < 2)
                {
                    throw new ArgumentException("The new time slot must be at least 2 hours ahead of the current time.");
                }
            }

            // Update the appointment
            appointment.StartDate = newStartDate;
            appointment.TimeSlot = appmnt.TimeSlot;
            appointment.Description = appmnt.Description;
            appointment.Note = appmnt.Note;

            // Save the changes to the database
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AppointmentDto>> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime minAvailableTime = currentDateTime.AddHours(2);

            // Generate all possible time slots (7 to 19)
            List<int> allTimeSlots = Enumerable.Range(7, 13).ToList();

            // Fetch existing appointments for the specified dentist and date
            var existingAppointments = await _context.Appointments
                .Where(a => a.DentId == dentId && a.StartDate.HasValue && a.StartDate.Value.Date == startDate.Date)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            // Filter out the unavailable slots
            List<int> availableSlots = allTimeSlots
                .Where(slot =>
                    !existingAppointments.Contains(slot) &&
                    startDate.Date.AddHours(slot) > minAvailableTime)
                .ToList();

            // Convert to AppointmentDto list
            List<AppointmentDto> availableTimeSlots = availableSlots.Select(slot => new AppointmentDto
            {
                DentId = dentId.ToString(),
                Date = startDate.ToString("yyyy-MM-dd"),
                TimeSlot = slot
            }).ToList();

            return availableTimeSlots;
        }


        //public async Task<List<int>> GetDentistAvailableTimeSlots(DateTime startDate, Guid dentId, Guid? tempDentId)
        //{
        //    // Ensure the parameters are passed as SQL parameters to avoid SQL injection vulnerabilities
        //    var startDateParam = new SqlParameter("@StartDate", startDate);
        //    var dentIdParam = new SqlParameter("@DentID", dentId);
        //    var tempDentIdParam = new SqlParameter("@TempDentID", tempDentId ?? (object)DBNull.Value);

        //    // Define the raw SQL query
        //    string sqlQuery = @"
        //    DECLARE @StartDate DATETIME2 = @StartDate;
        //    DECLARE @DentID UNIQUEIDENTIFIER = @DentID;
        //    DECLARE @TempDentID UNIQUEIDENTIFIER = @TempDentID;

        //    WITH UsedTimeSlots AS (
        //        SELECT Time_Slot
        //        FROM [dbo].[Appointment]
        //        WHERE Start_Date = @StartDate
        //          AND (
        //                (Temp_Dent_ID IS NULL AND Dent_ID = @DentID)
        //                OR
        //                (Temp_Dent_ID IS NOT NULL AND Temp_Dent_ID = @TempDentID)
        //              )
        //    )

        //    SELECT Time_Slot
        //    FROM (VALUES (7), (8), (9), (10), (11), (12), (13), (14), (15), (16), (17), (18), (19)) AS AllSlots(Time_Slot)
        //    WHERE Time_Slot NOT IN (SELECT Time_Slot FROM UsedTimeSlots);";

        //    // Execute the raw SQL query using Entity Framework Core
        //    var availableTimeSlots = await _context.Appointments
        //        .FromSqlRaw(sqlQuery, startDateParam, dentIdParam, tempDentIdParam)
        //        .Select(s => s.TimeSlot)
        //        .ToListAsync();

        //    // Convert the results to a list of integers
        //    var availableTimeSlotsList = availableTimeSlots.Cast<int>().ToList();

        //    return availableTimeSlotsList;
        //}
    }
}
