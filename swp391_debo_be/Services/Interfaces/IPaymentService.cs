using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IPaymentService
    {
        public ApiRespone Create(CreatePaymentDto createPaymentDto);

        public ApiRespone HandlePaymentResponse(VnpayPayResponse vnpayResponse);

        public ApiRespone GetPayment(string id);
    }
}
