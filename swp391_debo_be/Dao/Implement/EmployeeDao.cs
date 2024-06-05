using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Entity.Implement;
using System.Linq;

namespace swp391_debo_be.Dao.Implement
{
    public class EmployeeDao : IEmployeeDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());
        public List<User> GetDentistBasedOnTreamentId(int treatmentId)
        {
            var employees = _context.Employees
                .Where(e => e.Treats.Any(t => t.Id == treatmentId))
                .ToList();

            var resultEmployees = _context.Users
                                  .Where(u => employees.Select(e => e.Id).Contains(u.Id))
                                  .ToList();

            return resultEmployees;
        }
    }
}
