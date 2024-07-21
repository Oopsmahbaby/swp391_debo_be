using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("dentists")]
        public ActionResult<ApiRespone> GetDentistsByBranchId([FromQuery] int treatment, [FromQuery] int branch)
        {
            return _employeeService.GetDentistBasedOnTreamentId(treatment, branch);
        }
        [HttpGet("getallempwithbranch")]
        public async Task<IActionResult> GetEmployeeWithBranch([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _employeeService.GetEmployeeWithBranch(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [HttpGet("getempbyid/{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var response = await _employeeService.GetEmployeeById(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("getempwithbranchid/{id}")]
        public async Task<IActionResult> GetEmployeeWithBranchId(int id, [FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _employeeService.GetEmployeeWithBranchId(id, page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("dentandstaff")]
        public async Task<IActionResult> GetEmployee(int page, int limit)
        {
            var response = await _employeeService.GetEmployee(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("updateBranchForEmployee/{id}")]
        public async Task<IActionResult> UpdateBranchForEmployee(Guid id, CreateEmployeeDto employee)
        {
            var response = await _employeeService.UpdateBranchForEmployee(id, employee);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("dentist/patients")]
        public ActionResult<ApiRespone> GetPatientList([FromQuery] int page, [FromQuery] int limit)
        {
            string? userId = JwtProvider.GetUserId(Request);

            if (userId == null)
            {
               return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "User not found", Success = false, };
            }

            return _employeeService.GetPatientList(userId, page, limit);
        }

        [HttpPost("dentist/assignClinicTreatments")] 
        public ActionResult<ApiRespone> AssignClinicTreatment(AsssignClinicTreatmentDto dto)
        {
            return _employeeService.CreateClinicTreatmentsForDentist(dto);
        }

    }
}
