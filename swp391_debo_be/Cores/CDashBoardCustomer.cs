using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Cores
{
    public class CDashBoardCustomer
    {
        protected static IDashBoardCustomerRepository _dashboardRepo = new DashBoardCustomerRepository();

        static CDashBoardCustomer()
        {
            var context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());
            var context2 = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());
            _dashboardRepo = new DashBoardCustomerRepository(new DashBoardCustomerDao(context, context2));
        }

        public static Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id)
        {
            return _dashboardRepo.ViewAppointmentState(id);
        }

        public static Task<List<DashboardCustomerDto>> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            return _dashboardRepo.ViewTotalPaidAmountOfCustomer(id);
        }

        public static Task<DashboardAdminDto> ViewTotalRevenue()
        {
            return _dashboardRepo.ViewTotalRevenue();
        }

        public static Task<List<object>> ViewAppointmentStateByDentist(Guid id)
        {
            return _dashboardRepo.ViewAppointmentStateByDentist(id);
        }

        public static Task<List<object>> ViewTotalAppointmentEachMonthsByDentist(Guid id)
        {
            return _dashboardRepo.ViewTotalAppointmentEachMonthsByDentist(id);
        }

        public static Task<List<object>> ViewMonthlyRevenueForCurrentYear()
        {
            return _dashboardRepo.ViewMonthlyRevenueForCurrentYear();
        }

        public static Task<List<object>> CountAppointmentsByTreatmentCategory()
        {
            return _dashboardRepo.CountAppointmentsByTreatmentCategory();
        }
        public static Task<List<object>> CountAppointmentsByTreatment()
        {
            return _dashboardRepo.CountAppointmentsByTreatment();
        }
        public static Task<List<object>> EmployeeSalaryDistribution()
        {
            return _dashboardRepo.EmployeeSalaryDistribution();
        }
    }
}
