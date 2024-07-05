using swp391_debo_be.Constants;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IDashBoardCustomerService
    {
        public Task<ApiRespone> ViewAppointmentState(Guid id);
        public Task<ApiRespone> ViewTotalPaidAmountOfCustomer(Guid id);
        public Task<ApiRespone> ViewTotalRevenue();
        public Task<ApiRespone> ViewAppointmentStateByDentist(Guid id);
        public Task<ApiRespone> ViewTotalAppointmentEachMonthsByDentist(Guid id);
        public Task<ApiRespone> ViewMonthlyRevenueForCurrentYear();
        public Task<ApiRespone> CountAppointmentsByTreatmentCategory();
        public Task<ApiRespone> CountAppointmentsByTreatment();
    }
}
