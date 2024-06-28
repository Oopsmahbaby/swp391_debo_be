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
            _dashboardRepo = new DashBoardCustomerRepository(new DashBoardCustomerDao(context));
        }

        public static Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id)
        {
            return _dashboardRepo.ViewAppointmentState(id);
        }

        public static Task<DashboardCustomerDto> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            return _dashboardRepo.ViewTotalPaidAmountOfCustomer(id);
        }
    }
}
