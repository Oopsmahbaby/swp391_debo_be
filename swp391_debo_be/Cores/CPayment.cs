using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Cores
{
    public class CPayment
    {
        protected static readonly IPaymentRepository paymentRepository = new PaymentRepository();

        public static PaymentLinkDto Create(CreatePaymentDto createPaymentDto)
        {
            try
            {
                return paymentRepository.Create(createPaymentDto);
            }
            catch
            {
                throw;
            }
        }

        public static PaymenReturnDto HandlePaymentResponse(VnpayPayResponse vnpayResponse)
        {
            try
            {
                return paymentRepository.HandlePaymentResponse(vnpayResponse);
            }
            catch
            {
                throw;
            }
        }

        public static Payment? GetPaymentById(System.Guid id)
        {
            try
            {
                return paymentRepository.GetPaymentById(id);
            }
            catch
            {
                throw;
            }
        }
    }
}
