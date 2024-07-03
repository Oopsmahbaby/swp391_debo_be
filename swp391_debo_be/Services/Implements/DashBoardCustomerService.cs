using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class DashBoardCustomerService : IDashBoardCustomerService
    {
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

        public async Task<ApiRespone> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            try
            {
                var data = await CDashBoardCustomer.ViewTotalPaidAmountOfCustomer(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Data retrieved successfully.", Success = true };
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
