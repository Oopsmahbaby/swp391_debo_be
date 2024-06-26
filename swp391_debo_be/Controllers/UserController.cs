using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Web.Http;

namespace swp391_debo_be.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("profile")]
        [Authorize(Roles = SystemRole.Customer)]
        public IActionResult GetProfile()
        {
            return Ok(_userService.GetUsers());
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("createstaff")]
        public async Task<IActionResult> CreateNewStaff(EmployeeDto employee)
        {
            var response = await _userService.CreateNewStaff(employee);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("createdentist")]
        public async Task<IActionResult> CreateNewDent(EmployeeDto employee)
        {
            var response = await _userService.CreateNewDent(employee);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("createmanager")]
        public async Task<IActionResult> CreateNewManager(EmployeeDto employee)
        {
            var response = await _userService.CreateNewManager(employee);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("stafflist")]
        public async Task<IActionResult> ViewStaffList([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _userService.ViewStaffList(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("dentistlist")]
        public async Task<IActionResult> ViewDentList([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _userService.ViewDentList(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("managerlist")]
        public async Task<IActionResult> ViewManagerList([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _userService.ViewManagerList(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("customerlist")]
        public async Task<IActionResult> ViewCustomerList([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _userService.ViewCustomerList(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("userdetail")]
        public async Task<IActionResult> GetUserById2(Guid id)
        {
            var response = await _userService.GetUserById2(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("patient/isFirstTime")]
        public ActionResult<ApiRespone> firstTimeBooking()
        {
           var userId = JwtProvider.GetUserId("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjYzAyOWJlOC0yYzAwLTRhMTktOTU1Zi1mODhlZjQ0YWNhNWMiLCJlbWFpbCI6Im5ndXllbmxlQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbW9iaWxlcGhvbmUiOiIwOTAxMjMyMTIzIiwicm9sZSI6IkN1c3RvbWVyIiwibmJmIjoxNzE5MzcyMDU5LCJleHAiOjE3MTkzNzU2NTksImlhdCI6MTcxOTM3MjA1OX0.pP4jh4AbH8d2RdyytZ48dTvAPXUPJhAuP_A0PCth4p8");
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiRespone { Data = null, Message = "Authorization header is required", Success = false };
            }
            return Ok(_userService.firstTimeBooking(userId));
        }
    }
}
