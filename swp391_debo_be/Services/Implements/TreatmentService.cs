using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Collections.Generic;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class TreatmentService : ITreatmentService
    {
        public async Task<ApiRespone> addTreatmentAsync(TreatmentDto treatment)
        {
            var response = new ApiRespone();
            try
            {
                // -1 mean get all treatment 
                var existingTreatments = await CTreatment.getAllTreatmentAsync(1, -1);

                // Check if the treatment ID already exists
                if (existingTreatments.Any(t => t.Id == treatment.Id))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.Message = "ID cannot be duplicated";
                    return response;
                }
                var newTreat = await CTreatment.addTreatmentAsync(treatment);
                var treat = await CTreatment.getTreatmentAsync(newTreat);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = treat;
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
                var existingTreatments = await CTreatment.getTreatmentAsync(id);
                if (existingTreatments != null)
                {
                    await CTreatment.deleteTreatmentAsync(id);
                    response.StatusCode = HttpStatusCode.OK;
                    response.Data = existingTreatments;
                    response.Success = true;
                    response.Message = "Treatment data is deleted successfully.";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Success = false;
                    response.Message = "Treatment not found.";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> getAllTreatmentAsync(int page, int limit)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CTreatment.getAllTreatmentAsync(page, limit);
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
                var existingTreatments = await CTreatment.getTreatmentAsync(id);
                if (existingTreatments != null)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Data = existingTreatments;
                    response.Success = true;
                    response.Message = "Treatment data retrieved successfully.";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Success = false;
                    response.Message = "Treatment not found.";
                }
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
                await CTreatment.updateTreatmentAsync(id, treatment);
                var data = await CTreatment.getTreatmentAsync(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = data;
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
