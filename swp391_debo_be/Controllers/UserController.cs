﻿using Microsoft.AspNetCore.Mvc;
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
    }
}
