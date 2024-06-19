using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Constants;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IEmployeeService
    {
        public ActionResult<ApiRespone> GetDentistBasedOnTreamentId(int treatmentId);
        public Task<ApiRespone> GetEmployeeWithBranch(int page, int limit);
        public Task<ApiRespone> GetEmployeeById(Guid id);
    }
}
