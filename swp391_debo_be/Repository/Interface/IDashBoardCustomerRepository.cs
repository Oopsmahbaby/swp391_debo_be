using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IDashBoardCustomerRepository
    {
        public Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id);
        public Task<List<DashboardCustomerDto>> ViewTotalPaidAmountOfCustomer(Guid id);
        public Task<DashboardAdminDto> ViewTotalRevenue();
        public Task<List<object>> ViewAppointmentStateByDentist(Guid id);
        public Task<List<object>> ViewTotalAppointmentEachMonthsByDentist(Guid id);
    }
}
