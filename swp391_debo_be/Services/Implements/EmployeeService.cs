using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Helpers;
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

        public async Task<ApiRespone> GetEmployee(int page, int limit)
        {
            try
            {
                var data = await CEmployee.GetEmployee(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Employee data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetEmployeeById(Guid id)
        {
            try
            {
                var data = await CEmployee.GetEmployeeById(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Employee data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetEmployeeWithBranch(int page, int limit)
        {
            try
            {
                var data = await CEmployee.GetEmployeeWithBranch(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Employee data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetEmployeeWithBranchId(int id, int page, int limit)
        {
            try
            {
                var data = await CEmployee.GetEmployeeWithBranchId(id, page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Employee data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public ActionResult<ApiRespone> GetPatientList(string userId, int page, int limit)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid user id", Success = false};
                }

                if (PaginationValidation.ValidatePagination(page, limit))
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid page or limit", Success = false};
                }

                if (Guid.TryParse(userId, out Guid id))
                {
                    var result = CEmployee.GetPatientList(id, page, limit);

                    if (result == null)
                    {
                        return new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Not found", Success = false};
                    }

                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Data = result, Message = "Fetched patient list successfully", Success = true};
                } else
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = "Invalid user id", Success = false};
                }

            } catch (Exception e)
            {
                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.BadRequest, Message = e.Message, Success = false};
            }
        }

        public async Task<ApiRespone> UpdateBranchForEmployee(Guid id, CreateEmployeeDto employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Employee ID not found", Success = false };
                }
                var data = await CEmployee.GetEmployeeById(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Employee not found", Success = false }; ;
                }
                await CEmployee.UpdateBranchForEmployee(id, employee);
                var updEmp = await CEmployee.GetEmployeeById(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updEmp, Message = "Employee data updated successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }
    }
}
