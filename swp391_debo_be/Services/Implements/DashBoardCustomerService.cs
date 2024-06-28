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
            var response = new ApiRespone();
            try
            {
                var data = await CDashBoardCustomer.ViewAppointmentState(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = new { list = data, total = data.Count };
                response.Success = true;
                response.Message = "Data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CDashBoardCustomer.ViewTotalPaidAmountOfCustomer(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = data;
                response.Success = true;
                response.Message = "Data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
