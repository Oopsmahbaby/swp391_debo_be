using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Dao.Interface
{
    public interface IDashBoardCustomerDao
    {
        public Task<List<DashboardCustomerDto>> ViewAppointmentState(Guid id);
        public Task<List<DashboardCustomerDto>> ViewTotalPaidAmountOfCustomer(Guid id);
        public Task<DashboardAdminDto> ViewTotalRevenue();
        public Task<List<object>> ViewAppointmentStateByDentist(Guid id);
        public Task<List<object>> ViewTotalAppointmentEachMonthsByDentist(Guid id);
        public Task<List<object>> ViewMonthlyRevenueForCurrentYear();
        public Task<List<object>> CountAppointmentsByTreatmentCategory();
        public Task<List<object>> CountAppointmentsByTreatment();
        public Task<List<object>> EmployeeSalaryDistribution();
    }
}
