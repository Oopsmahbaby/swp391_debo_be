using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class TreatmentService
    {
        private readonly CTreatment _treatCores;

        public TreatmentService(CTreatment treatCores) 
        {
            _treatCores = treatCores;
        }
        public async Task<ApiRespone> addTreatmentAsync(TreatmentDto treatment)
        {
            var response = new ApiRespone();
            try
            {
                var newTreat = await _treatCores.addTreatmentAsync(treatment);
                var treat = await _treatCores.getTreatmentAsync(newTreat);
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.Message = "Treatment data is added successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> deleteTreatmentAsync(int id)
        {
            var response = new ApiRespone();
            try
            {
                await _treatCores.deleteTreatmentAsync(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.Message = "Treatment data is deleted successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> getAllTreatmentAsync()
        {
            var response = new ApiRespone();
            try
            {
                var data = await _treatCores.getAllTreatmentAsync();
                response.StatusCode = HttpStatusCode.OK;
                response.Data = data;
                response.Success = true;
                response.Message = "Treatment data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> getTreatmentAsync(int id)
        {
            var response = new ApiRespone();
            try
            {
                var data = await _treatCores.getTreatmentAsync(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = data;
                response.Success = true;
                response.Message = "Treatment data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> updateTreatmentAsync(int id, TreatmentDto treatment)
        {
            var response = new ApiRespone();
            try
            {
                if (id != treatment.Id)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Success = false;
                    response.Message = "This Treatment does not exist in system";
                }
                await _treatCores.updateTreatmentAsync(id,treatment);
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.Message = "Treatment data updated successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
