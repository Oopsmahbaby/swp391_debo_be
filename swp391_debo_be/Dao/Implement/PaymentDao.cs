using Azure.Core;
using Microsoft.Extensions.Options;
using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using System.Net.WebSockets;
using System.Security.Claims;

namespace swp391_debo_be.Dao.Implement
{
    public class CurrentUserHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserHelper()
        {
        }

        public CurrentUserHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string? UserId => httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

    }
    public class PaymentDao : IPaymentDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context(new Microsoft.EntityFrameworkCore.DbContextOptions<DeboDev02Context>());
        private readonly CurrentUserHelper currentUser = new();
        private readonly VnpayConfig vnpayConfig = new();
        public PaymentDao()
        {
        }

        public PaymentDao(DeboDev02Context context)
        {
            _context = context;
        }
        public PaymentLinkDto? Create(CreatePaymentDto createPaymentDto)
        {
            bool isGeneralCheckup = false;
            var paymentUrl = string.Empty;
            Guid paymentId = Guid.Empty;

            foreach (string appointmentId in createPaymentDto.ListAppointmentId)
            {
                Appointment? appointment = _context.Appointments.FirstOrDefault(a => a.Id.ToString() == appointmentId);

                if (appointment == null)
                {
                    return null;
                }

                if (appointment.TreatId == 8)
                {
                    isGeneralCheckup = true;
                }
            }

            if (isGeneralCheckup == false)
            {

                Payment payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    ExpireDate = DateTime.UtcNow.AddMinutes(15),
                    PaymentContent = createPaymentDto.PaymentContent,
                    PaymentCurrency = createPaymentDto.PaymentCurrency,
                    PaymentDate = DateTime.UtcNow,
                    PaymentLanguage = createPaymentDto.PaymentLanguage,
                    RequiredAmount = createPaymentDto.RequiredAmount,
                    PaymentStatus = "Pending",
                };
                _context.Payments.Add(payment);
                _context.SaveChanges();
            
                // Handle update paymentId for appointments
            

                var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                vnpayConfig.TmnCode, DateTime.Now, "127.0.0.1", createPaymentDto.RequiredAmount ?? 0, createPaymentDto.PaymentCurrency ?? string.Empty,
                               "other", createPaymentDto.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, payment.Id.ToString());
                paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                paymentId = payment.Id;
            }


            for (int i = 0; i < createPaymentDto.ListAppointmentId.Count; i++)
            {
                Appointment? appointment = _context.Appointments.FirstOrDefault(a => a.Id.ToString() == createPaymentDto.ListAppointmentId[i]);
                if (appointment == null)
                {
                    return null;
                }

                appointment.PaymentId = paymentId;
                _context.Update(appointment);
                _context.SaveChanges();
            }

            var result = new PaymentLinkDto
             {
                IsGeneralCheckup = isGeneralCheckup,
                PaymentId = paymentId,
                PaymentUrl = paymentUrl
            };

            return result;
        }

        public PaymenReturnDto HandleVnpayPaymentReturnProcess(VnpayPayResponse vnpayPayResponse)
        {
            var result = new PaymenReturnDto();
            try
            {
                var resultData = new PaymenReturnDto();
                var isValidSignature = vnpayPayResponse.IsValidSignature(vnpayConfig.HashSecret);

                if (isValidSignature)
                {
                    var payment = _context.Payments.FirstOrDefault(p => p.Id.ToString() == vnpayPayResponse.vnp_TxnRef);
                    if (payment == null)
                    {
                        resultData.PaymentStatus = "11";
                        resultData.PaymentMessage = "Payment Not Found";
                    }

                    if (vnpayPayResponse.vnp_ResponseCode == "00")
                    {
                        resultData.PaymentStatus = "00";
                        payment.PaymentStatus = "Success";
                        resultData.PaymentId = payment.Id.ToString();
                        ///TODO: Make signature
                        resultData.Signature = Guid.NewGuid().ToString();
                        resultData.Amount = payment.RequiredAmount;
                        resultData.PaymentMessage = "Payment Success";
                        resultData.PaymentDate = payment.PaymentDate.ToString();

                        var appoinment = _context.Appointments.FirstOrDefault(a => a.PaymentId == payment.Id);

                        appoinment.Status = "future";
                        appoinment.PaymentId = payment.Id;
                        _context.Update(appoinment);
                        _context.SaveChanges();

                    }
                    else
                    {
                        resultData.PaymentStatus = "10";
                        payment.PaymentStatus = "Failed";
                        resultData.PaymentMessage = "Payment Failed";
                    }
                    PaymentTransaction paymentTransaction = new PaymentTransaction()
                    {
                        Id = Guid.NewGuid(),
                        PaymentId = payment.Id,
                        TranAmount = payment.RequiredAmount,
                        TranDate = DateTime.UtcNow,
                        TranStatus = payment.PaymentStatus
                    };
                    _context.PaymentTransactions.Add(paymentTransaction);
                    _context.Update(payment);
                    _context.SaveChanges();

                } else
                {
                    resultData.PaymentStatus = "99";
                    resultData.PaymentMessage = "Invalid signature in response";
                }

                return resultData;
            } catch (Exception e)
            {
                return null;
            }
        }

        public Payment? GetPaymentById(Guid id)
        {
            return _context.Payments.FirstOrDefault(p => p.Id == id);
        }
    }
}
