using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Cores
{
    public class CEmployee
    {
        protected static IEmployeeRepository _employeeRepo = new EmployeeRepository();

        public static List<User> GetDentistBasedOnTreamentId(int treatmentId)
        {
            return _employeeRepo.GetDentistBasedOnTreamentId(treatmentId);
        }
    }
}
