using Amazon.S3;
using Amazon.S3.Model;
using Azure;
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
        private readonly IAmazonS3 _s3Client;

        public UserController(IUserService userService, IAmazonS3 s3Client)
        {
            _userService = userService;
            _s3Client = s3Client;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("profile")]
        [Authorize(Roles = SystemRole.Customer)]
        public IActionResult GetProfile()
        {
            return Ok(_userService.GetUsers());
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("createstaff")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateNewStaff(IFormFile? file, [FromForm] EmployeeDto employee)
        {
            string bucketName = "swp391-bucket";

            // Check if bucket exists
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            if (file != null && file.Length > 0)
            {
                // Upload the file to S3
                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                };
                await _s3Client.PutObjectAsync(request);

                // Generate the URL for the uploaded file
                string fileUrl = $"https://{bucketName}.s3.amazonaws.com/{file.FileName}";

                // Set the avatar URL in the employee DTO
                employee.Avt = fileUrl;
            }

            if (!employee.Id.HasValue)
            {
                employee.Id = new Guid(Guid.NewGuid().ToString());
            }

            // Save the new staff information
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
        [Microsoft.AspNetCore.Mvc.HttpGet("userdetail/{id}")]
        public async Task<IActionResult> GetUserById2(Guid id)
        {
            var response = await _userService.GetUserById2(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, EmployeeDto emp)
        {
            var response = await _userService.UpdateUser(id, emp);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}
