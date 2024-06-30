using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace swp391_debo_be.Services.Implements
{
    public class TreatmentService : ITreatmentService
    {
        public async Task<ApiRespone> addTreatmentAsync(TreatmentDto treatment)
        {
            try
            {
                // -1 mean get all treatment 
                var existingTreatments = await CTreatment.getAllTreatmentAsync(1, -1);

                // Check if the treatment ID already exists
                if (existingTreatments.Any(t => t.Id == treatment.Id))
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "ID cannot be duplicated", Success = false };
                }
                var newTreat = await CTreatment.addTreatmentAsync(treatment);
                var treat = await CTreatment.getTreatmentAsync(newTreat);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = treat, Message = "Treatment data is added successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> deleteTreatmentAsync(int id)
        {
            try
            {
                var existingTreatments = await CTreatment.getTreatmentAsync(id);
                if (existingTreatments != null)
                {
                    await CTreatment.deleteTreatmentAsync(id);
                    return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingTreatments, Message = "Treatment data is deleted successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Treatment not found", Success = false };
                }

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> activeTreatmentAsync(int id)
        {
            try
            {
                var existingTreatments = await CTreatment.getTreatmentAsync(id);
                if (existingTreatments != null)
                {
                    await CTreatment.activeTreatmentAsync(id);
                    return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingTreatments, Message = "Treatment data is activated successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Treatment not found", Success = false };
                }

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> getAllTreatmentAsync(int page, int limit)
        {
            try
            {
                var data = await CTreatment.getAllTreatmentAsync(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Treatment data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> getTreatmentAsync(int id)
        {
            try
            {
                var existingTreatments = await CTreatment.getTreatmentAsync(id);
                if (existingTreatments != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingTreatments, Message = "Treatment data retrieved successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Treatment not found", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public ActionResult<ApiRespone> GetTreatmentsBasedBranchId(int branchId)
        {
            try
            {
                var data = CTreatment.GetTreatmentsBasedBranchId(branchId);

                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Data = null, Success = false, Message = "This branch does not have any treatments" };
                }

                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Success = true, Message = "Fetched treatments within constraints successfully" };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> updateTreatmentAsync(int id, TreatmentDto treatment)
        {
            try
            {
                if (id != treatment.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Treatment ID mismatch", Success = false };
                }
                var data = await CTreatment.getTreatmentAsync(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Treatment not found or inactive.", Success = false };
                }
                await CTreatment.updateTreatmentAsync(id, treatment);
                var updTreat = await CTreatment.getTreatmentAsync(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updTreat, Message = "Treatment data updated successfully.", Success = true };

            }
            catch (InvalidOperationException ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

    }
}
