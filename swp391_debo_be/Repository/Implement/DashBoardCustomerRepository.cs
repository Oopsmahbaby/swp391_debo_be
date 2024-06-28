using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Repository.Implement
{
    public class DashBoardCustomerRepository : IDashBoardCustomerRepository
    {
        private readonly IDashBoardCustomerDao _dashBoardDao;

        public DashBoardCustomerRepository()
        {
            _dashBoardDao = new DashBoardCustomerDao();
        }

        public DashBoardCustomerRepository(IDashBoardCustomerDao dashBoardDao) 
        {
            _dashBoardDao = dashBoardDao;
        }
        public Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id)
        {
            return _dashBoardDao.ViewAppointmentState(id);
        }

        public Task<DashboardCustomerDto> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            return _dashBoardDao.ViewTotalPaidAmountOfCustomer(id);
        }
    }
}
