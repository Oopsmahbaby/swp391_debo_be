using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Cores;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Implement
{
    public class DashBoardCustomerDao : IDashBoardCustomerDao
    {
        private readonly DeboDev02Context _context;
        private readonly DeboDev02Context _context2;

        public DashBoardCustomerDao()
        {
            _context = new DeboDev02Context();
            _context2 = new DeboDev02Context();
        }

        public DashBoardCustomerDao(DeboDev02Context context, DeboDev02Context context2)
        {
            _context = context;
            _context2 = context2;
        }
        public async Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id)
        {
            var result = await _context.Appointments
            .Where(a => a.CusId == id && a.Cus.Role == 5)
            .GroupBy(a => a.Status)
            .Select(g => new DashboardCustomerDto
            {
                CusId = id,
                Status = g.Key,
                AppointmentCount = g.Count()
            })
            .ToListAsync();

            return result;
        }

        public async Task<List<object>> ViewAppointmentStateByDentist(Guid id)
        {
            var appointmentStates = await _context.Appointments
            .Where(a => a.TempDentId == id || (a.TempDentId == null && a.DentId == id))
            .GroupBy(a => a.Status)
            .Select(g => new
            {
                Status = g.Key,
                TotalAppointments = g.Count()
            })
            .OrderBy(x => x.Status)
            .ToListAsync();

            return appointmentStates.Cast<object>().ToList();
        }

        public async Task<List<object>> ViewTotalAppointmentEachMonthsByDentist(Guid id)
        {
            var appointmentCounts = await _context2.Appointments
            .Where(a => (a.TempDentId == id || (a.TempDentId == null && a.DentId == id))
                        && a.Status != "Canceled" && a.Status != "Pending")
            .GroupBy(a => new { a.StartDate.Value.Year, a.StartDate.Value.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalAppointments = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

            return appointmentCounts.Cast<object>().ToList();
        }

        public async Task<List<DashboardCustomerDto>> ViewTotalPaidAmountOfCustomer(Guid cusId)
        {
            var monthlyPayments = await (from a in _context2.Appointments
                                         join p in _context2.Payments on a.PaymentId equals p.Id
                                         join t in _context2.ClinicTreatments on a.TreatId equals t.Id
                                         where a.CusId == cusId && p.PaymentStatus == "Success"
                                         group new { a, p, t } by new
                                         {
                                             a.CusId,
                                             Year = p.PaymentDate.HasValue ? p.PaymentDate.Value.Year : 0,
                                             Month = p.PaymentDate.HasValue ? p.PaymentDate.Value.Month : 0,
                                             a.TreatId,
                                             t.Name
                                         } into g
                                         select new
                                         {
                                             g.Key.CusId,
                                             g.Key.Year,
                                             g.Key.Month,
                                             g.Key.TreatId,
                                             g.Key.Name,
                                             TotalPaidAmount = (double)g.Sum(x => x.p.RequiredAmount)
                                         }).ToListAsync();

            var result = new List<DashboardCustomerDto>();
            double runningTotal = 0;

            foreach (var payment in monthlyPayments.OrderBy(p => p.Year).ThenBy(p => p.Month).ThenBy(p => p.TreatId))
            {
                runningTotal += payment.TotalPaidAmount;
                result.Add(new DashboardCustomerDto
                {
                    CusId = payment.CusId,
                    Year = payment.Year,
                    Month = payment.Month,
                    TreatId = payment.TreatId,
                    TreatmentName = payment.Name,
                    TotalPaidAmount = payment.TotalPaidAmount,
                    RunningTotal = runningTotal
                });
            }

            return result;
        }

        public async Task<DashboardAdminDto> ViewTotalRevenue()
        {
            var result = await (from appointment in _context.Appointments
                                      join treatment in _context.ClinicTreatments
                                      on appointment.TreatId equals treatment.Id
                                      select new
                                      {
                                          treatment.Price,
                                          appointment.Id
                                      })
                        .GroupBy(x => 1) // Group all results to calculate the aggregate functions
                        .Select(g => new
                        {
                            TotalRevenue = g.Sum(x => x.Price),
                            TotalAppointment = g.Count()
                        })
                        .FirstOrDefaultAsync();

            return new DashboardAdminDto 
            { 
                TotalRevenue = result.TotalRevenue,
                TotalAppointment = result.TotalAppointment,
            };
        }


    }
}
