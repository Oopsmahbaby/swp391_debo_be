using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Services.Interfaces;

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
    }
}
