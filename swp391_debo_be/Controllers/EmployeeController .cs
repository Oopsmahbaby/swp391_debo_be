using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }

        [HttpGet("dentists")]
        public ActionResult<ApiRespone> GetDentistsByBranchId([FromQuery] int treatment)
        {
            return _employeeService.GetDentistBasedOnTreamentId(treatment);
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var response = await _employeeService.GetEmployeeById(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}
