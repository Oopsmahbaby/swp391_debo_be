using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Dao.Interface
{
    public interface IDashBoardCustomerDao
    {
        public Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id);
        public Task<DashboardCustomerDto> ViewTotalPaidAmountOfCustomer(Guid id);

        public Task<DashboardAdminDto> ViewTotalRevenue();
    }
}
