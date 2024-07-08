using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Services.Implements;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/branch")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly BranchService _branchService;
        private readonly IAmazonS3 _s3Client;

        public BranchController(BranchService branchService, IAmazonS3 s3Client) 
        {
            _branchService = branchService;
            _s3Client = s3Client;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranch([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _branchService.getAllBranchAsync(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchByID(int id)
        {
            var response = await _branchService.getBranchAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("branchmanager/{id}")]
        public async Task<IActionResult> GetManagerBranchAsync(Guid id)
        {
            var response = await _branchService.getManagerBranchAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("branchappointment/{id}")]
        public async Task<IActionResult> GetAppointmentBranchAsync(Guid id)
        {
            var response = await _branchService.getAppointmentBranchAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("{id}/upload-pic-branch")]
        public async Task<IActionResult> UploadPicBranch(int id, [FromForm] BranchDto branch, IFormFile? file)
        {
            string bucketName = "swp391-bucket";
            var currentBranch = await CBranch.getBranchAsync(id);
            if (file != null && file.Length > 0)
            {
                if (!string.IsNullOrEmpty(currentBranch.Avt))
                {
                    // Extract the existing file key from the URL
                    var existingFileKey = new Uri(currentBranch.Avt).AbsolutePath.TrimStart('/');

                    // Delete the existing avatar file from S3
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = existingFileKey
                    };
                    await _s3Client.DeleteObjectAsync(deleteRequest);
                }
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
                branch.Avt = fileUrl;
            }
            else
            {
                var existingFileKey = new Uri(currentBranch.Avt).AbsolutePath.TrimStart('/');

                // Delete the existing avatar file from S3
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = existingFileKey
                };
                await _s3Client.DeleteObjectAsync(deleteRequest);
                branch.Avt = null;
            }
            var response = await _branchService.UploadPicBranch(id, branch);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBranch(BranchDto model)
        {
            var response = await _branchService.addBranchAsync(model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var response = await _branchService.deleteBranchAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchDto model)
        {
            var response = await _branchService.updateBranchAsync(id, model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

        }
    }
}
