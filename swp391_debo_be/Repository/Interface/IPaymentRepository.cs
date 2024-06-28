using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IPaymentRepository
    {
        public PaymentLinkDto Create(CreatePaymentDto createPaymentDto);
        public PaymenReturnDto HandlePaymentResponse(VnpayPayResponse vnpayResponse);
    }
}
