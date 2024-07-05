using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    [Route("api/dashboard")]
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

        [HttpGet("totalpaid/{id}")]
        public async Task<IActionResult> ViewTotalPaidAmountOfCustomer(Guid id)
        {
            var response = await _dbcus.ViewTotalPaidAmountOfCustomer(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("totalrevenue")]
        public async Task<IActionResult> ViewTotalRevenue()
        {
            var response = await _dbcus.ViewMonthlyRevenueForCurrentYear();
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("dentist/appointmentstate/{id}")]
        public async Task<IActionResult> ViewAppointmentStateByDentist(Guid id)
        {
            var response = await _dbcus.ViewAppointmentStateByDentist(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("dentist/totalPatient/{id}")]
        public async Task<IActionResult> ViewTotalAppointmentEachMonthsByDentist(Guid id)
        {
            var response = await _dbcus.ViewTotalAppointmentEachMonthsByDentist(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("distribution/treatment")]
        public async Task<IActionResult> CountAppointmentsByTreatment()
        {
            var response = await _dbcus.CountAppointmentsByTreatment();
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("distribution/categories")]
        public async Task<IActionResult> CountAppointmentsByTreatmentCategory()
        {
            var response = await _dbcus.CountAppointmentsByTreatmentCategory();
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}
