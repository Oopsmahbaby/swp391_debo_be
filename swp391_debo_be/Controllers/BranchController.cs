using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Dto.Implement;
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

        public BranchController(BranchService branchService) 
        {
            _branchService = branchService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBranch([FromQuery] int page = 1, [FromQuery] int limit = 5)
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
        public async Task<IActionResult> UpdateBranch(int id, BranchDto model)
        {
            var response = await _branchService.updateBranchAsync(id, model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

        }
    }
}
