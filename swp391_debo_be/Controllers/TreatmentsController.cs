using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Interface;
using swp391_debo_be.Services.Implements;
using swp391_debo_be.Services.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace swp391_debo_be.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/treatments")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly ITreatmentService _treatService;
        private readonly DeboDev02Context _context;

        public TreatmentsController(ITreatmentService treatService, DeboDev02Context context)
        {
            _treatService = treatService;
            _context = context;
        }

        [HttpGet]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllTreatment([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            var response = await _treatService.getAllTreatmentAsync(page, limit);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTreatmentByID(int id)
        {
            var response = await _treatService.getTreatmentAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTreatment(TreatmentDto model)
        {
            var response = await _treatService.addTreatmentAsync(model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var response = await _treatService.deleteTreatmentAsync(id);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTreatment(int id, [FromBody] TreatmentDto model)
        {
            var response = await _treatService.updateTreatmentAsync(id, model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

        }

        [HttpGet("branch/{branchId}")]
        public ActionResult<ApiRespone> GetTreatmentBasedBranchId(int branchId)
        {
            var response = _treatService.GetTreatmentsBasedBranchId(branchId);
            return response;
        }

        [HttpGet("getalltreatmentTest")]
        public async Task<IActionResult> GetAllTreatmentTest([FromQuery] int page = 0, [FromQuery] int limit = 5)
        {
            try
            {
                var query = _context.ClinicTreatments
                                    .Where(t => t.Status == true);

                if (limit > 0)
                {
                    query = query.Skip(page * limit)
                                 .Take(limit);
                }

                var treatments = await query.ToListAsync();

                var treatmentDtos = treatments.Select(t => new TreatmentDto
                {
                    Id = t.Id,
                    Category = t.Category,
                    Name = t.Name,
                    Description = t.Description,
                    Price = t.Price,
                }).ToList();

                return Ok(new ApiRespone
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new { list = treatmentDtos, total = treatmentDtos.Count },
                    Message = "Treatment data retrieved successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiRespone
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
