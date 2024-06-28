using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Implement
{
    public class DashBoardCustomerDao : IDashBoardCustomerDao
    {
        private readonly DeboDev02Context _context;

        public DashBoardCustomerDao()
        {
            _context = new DeboDev02Context();
        }

        public DashBoardCustomerDao(DeboDev02Context context)
        {
            _context = context;
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

        public async Task<DashboardCustomerDto> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            var result = await _context.Appointments
            .Where(a => a.CusId == id && a.Cus.Role == 5 && a.Payment.PaymentStatus == "Success")
            .GroupBy(a => a.CusId)
            .Select(g => new DashboardCustomerDto
            {
                CusId = g.Key,
                TotalPaidAmount = g.Sum(a => (double?)a.Payment.PaidAmount) ?? 0
            })
            .FirstOrDefaultAsync();

            return result;
        }
    }
}
