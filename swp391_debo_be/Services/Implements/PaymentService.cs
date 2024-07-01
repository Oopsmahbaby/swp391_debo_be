using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        public ApiRespone Create(CreatePaymentDto createPaymentDto)
        {
            var result = CPayment.Create(createPaymentDto);

            if (result == null)
            {
                return new ApiRespone
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Create payment failed",
                    Data = null,
                    Success = false
                };
            }
            return new ApiRespone
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Message = "Create payment successfully",
                Data = result,
                Success = true
            };
        }

        public PaymenReturnDto HandlePaymentResponse(VnpayPayResponse vnpayResponse)
        {
            var result = CPayment.HandlePaymentResponse(vnpayResponse);

            return result;
        }

        public ApiRespone GetPayment(string id)
        {
            var result = CPayment.GetPaymentById(System.Guid.Parse(id));

            if (result == null)
            {
                return new ApiRespone
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Payment not found",
                    Data = null,
                    Success = false
                };
            }

            return new ApiRespone
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Get payment successfully",
                Data = result,
                Success = true
            };
        }
    }
}
