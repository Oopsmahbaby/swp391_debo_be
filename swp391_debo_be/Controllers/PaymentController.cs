using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Config.VnPay;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Extensions;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public ActionResult<ApiRespone> CreatePayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            return _paymentService.Create(createPaymentDto);
        }

        [HttpGet("vnpay-return")]
        public IActionResult VnpayReturn([FromQuery] VnpayPayResponse response)
        {

            return Redirect("http://localhost:5173/patient/booking/payment-status");
        }

        [HttpGet("{id}/status")]
        public ActionResult<ApiRespone> GetPaymentStatus([FromRoute] string id)
        {
            return Ok(_paymentService.GetPayment(id));
        }
    }
}
