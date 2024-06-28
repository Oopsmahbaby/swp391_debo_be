using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardCustomerController : ControllerBase
    {
        private readonly IDashBoardCustomerService _dbcus;

        public DashBoardCustomerController(IDashBoardCustomerService dashBoardCustomerService)
        {
            _dbcus = dashBoardCustomerService;
        }
        [HttpGet("appointmentstate/{id}")]
        public async Task<IActionResult> ViewAppointmentState(Guid id)
        {
            var response = await _dbcus.ViewAppointmentState(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet]
        public async Task<IActionResult> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            var response = await _dbcus.ViewTotalPaidAmountOfCustomer(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}
