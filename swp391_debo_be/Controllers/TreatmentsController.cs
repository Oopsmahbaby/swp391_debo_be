using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Interface;
using swp391_debo_be.Services.Implements;
using System.Net;

namespace swp391_debo_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly TreatmentService _treatService;

        public TreatmentsController(TreatmentService treatService)
        {
            _treatService = treatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTreatment()
        {
            var response = await _treatService.getAllTreatmentAsync();
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
        public async Task<IActionResult> UpdateTreatment(int id, TreatmentDto model)
        {
            var response = await _treatService.updateTreatmentAsync(id,model);
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };

        }
    }
}
