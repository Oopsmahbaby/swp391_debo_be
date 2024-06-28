using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IDashBoardCustomerRepository
    {
        public Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id);
        public Task<DashboardCustomerDto> ViewTotalPaidAmountOfCustomer(Guid id);
    }
}
