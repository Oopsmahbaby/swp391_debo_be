using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IPaymentRepository
    {
        public PaymentLinkDto Create(CreatePaymentDto createPaymentDto);
        public PaymenReturnDto HandlePaymentResponse(VnpayPayResponse vnpayResponse);
        public Payment? GetPaymentById(Guid id);
    }
}
