﻿using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        public ActionResult<ApiRespone> GetDentistBasedOnTreamentId(int treatmentId)
        {
            try
            {

                if (treatmentId <= 0)
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid treatment id", Success = false};
                }
                var result = CEmployee.GetDentistBasedOnTreamentId(treatmentId);

                if (result == null)
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Not found", Success = false};
                }

                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched dentists within constraints successfully", Success = true};

            } catch (Exception e)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = e.Message, Success = false};
            }
        }

        public async Task<ApiRespone> GetEmployeeById(Guid id)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CEmployee.GetEmployeeById(id);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = data;
                response.Success = true;
                response.Message = "Employee data retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ApiRespone> GetEmployeeWithBranch(int page, int limit)
        {
            var response = new ApiRespone();
            try
            {
                var data = await CEmployee.GetEmployeeWithBranch(page, limit);
                response.StatusCode = HttpStatusCode.OK;
                response.Data = new { list = data, total = data.Count };
                response.Success = true;
                response.Message = "Employee data retrieved successfully.";
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
