using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class DashBoardCustomerService : IDashBoardCustomerService
    {
        public async Task<ApiRespone> CountAppointmentsByTreatment()
        {
            try
            {
                var data = await CDashBoardCustomer.CountAppointmentsByTreatment();
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CountAppointmentsByTreatmentAndBranchId(int id)
        {
            try
            {
                var data = await CDashBoardCustomer.CountAppointmentsByTreatmentAndBranchId(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CountAppointmentsByTreatmentCategory()
        {
            try
            {
                var data = await CDashBoardCustomer.CountAppointmentsByTreatmentCategory();
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CountAppointmentsByTreatmentCategoryAndBranchId(int id)
        {
            try
            {
                var data = await CDashBoardCustomer.CountAppointmentsByTreatmentCategoryAndBranchId(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> EmployeeSalaryDistribution()
        {
            try
            {
                var data = await CDashBoardCustomer.EmployeeSalaryDistribution();
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> TotalRevenueOfBranchId(int id)
        {
            try
            {
                var data = await CDashBoardCustomer.TotalRevenueOfBranchId(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewAppointmentState(Guid id)
        {
            try
            {
                var data = await CDashBoardCustomer.ViewAppointmentState(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewAppointmentStateByDentist(Guid id)
        {
            try
            {
                var data = await CDashBoardCustomer.ViewAppointmentStateByDentist(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewMonthlyRevenueForCurrentYear()
        {
            try
            {
                var data = await CDashBoardCustomer.ViewMonthlyRevenueForCurrentYear();
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewTotalAppointmentEachMonthsByDentist(Guid id)
        {
            try
            {
                var data = await CDashBoardCustomer.ViewTotalAppointmentEachMonthsByDentist(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            try
            {
                var data = await CDashBoardCustomer.ViewTotalPaidAmountOfCustomer(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewTotalRevenue()
        {
            try
            {
                var data = await CDashBoardCustomer.ViewTotalRevenue();
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Data retrieved successfully.", Success = true };
            }
            catch(Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }
    }
}
