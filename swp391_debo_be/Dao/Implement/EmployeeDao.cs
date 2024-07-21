using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using System.Linq;

namespace swp391_debo_be.Dao.Implement
{
    public class EmployeeDao : IEmployeeDao
    {
        private  DeboDev02Context _context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());
        public List<User> GetDentistBasedOnTreamentId(int treatmentId, int branchId)
        {
            var employees = _context.Employees
                .Where(e => e.BrId == branchId)
                .Where(e => e.Treats.Any(t => t.Id == treatmentId))
                .ToList();

            var resultEmployees = _context.Users
                                  .Where(u => employees.Select(e => e.Id).Contains(u.Id))
                                  .ToList();

            return resultEmployees;
        }

        public async Task<List<CreateEmployeeDto>> GetEmployee(int page, int limit)
        {
            var result = await (from u in _context.Users
                                join e in _context.Employees on u.Id equals e.Id
                                where e.Type != 2
                                select new CreateEmployeeDto
                                {
                                    Id = e.Id,
                                    Name = u.FirstName + " " + u.LastName,
                                    BrId = e.BrId,
                                    Type = e.Type,
                                    Salary = e.Salary
                                }).Skip(page * limit).Take(limit).ToListAsync();
            return result;
        }

        public async Task<CreateEmployeeDto> GetEmployeeById(Guid id)
        {
            var result = await (from u in _context.Users
                                join e in _context.Employees on u.Id equals e.Id
                                where u.Id == id
                                select new CreateEmployeeDto
                                {
                                    Id = e.Id,
                                    Name = u.FirstName + " " + u.LastName,
                                    BrId = e.BrId,
                                    Type = e.Type,
                                    Salary = e.Salary
                                }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<List<CreateEmployeeDto>> GetEmployeeWithBranch(int page, int limit)
        {
            var result = await (from u in _context.Users
                                join e in _context.Employees on u.Id equals e.Id
                                select new CreateEmployeeDto
                                {
                                    Id = e.Id,
                                    Name = u.FirstName + " " + u.LastName,
                                    BrId = e.BrId,
                                    Type = e.Type,
                                    Salary = e.Salary
                                }).Skip(page * limit).Take(limit).ToListAsync();

            return result;
        }

        public async Task<List<CreateEmployeeDto>> GetEmployeeWithBranchId(int id, int page, int limit)
        {
            var result = await (from u in _context.Users
                                join e in _context.Employees on u.Id equals e.Id
                                where e.BrId == id
                                select new CreateEmployeeDto
                                {
                                    Id = e.Id,
                                    Name = u.FirstName + " " + u.LastName,
                                    BrId = e.BrId,
                                    Type = e.Type,
                                    Salary = e.Salary
                                }).Skip(page * limit).Take(limit).ToListAsync();

            return result;
        }

        public object GetPatientList(Guid id, int page, int limit)
        {
            List<Appointment> appointments = _context.Appointments
                                .Where(a => a.DentId == id)
                                .ToList();
            List<User> users = new List<User>();
            if (limit == -1)
            {
                users = _context.Users
                                .Where(u => appointments.Select(a => a.CusId).Contains(u.Id))
                                .ToList();
            }
            users = _context.Users
                                .Where(u => appointments.Select(a => a.CusId).Contains(u.Id))
                                .Skip(page * limit)
                                .Take(limit)
                                .ToList();

            List<object> list = new List<object>();

            foreach (var user in users)
            {
                List<Appointment> apps = appointments.Where(a => a.DentId == id && a.CusId == user.Id && a.Status != "pending" && a.Status != "canceled").ToList();

                Appointment? nextApp = apps.Where(a => a.StartDate > DateTime.Now).OrderBy(a => a.StartDate).FirstOrDefault();

                DateTime? nextAppointment = nextApp?.StartDate;

                int? timeSlot = nextApp?.TimeSlot;

                if (nextApp == null)
                {
                    timeSlot = 0;
                }

                DateTime? lastAppointment = apps.Where(a => a.StartDate < DateTime.Now).OrderByDescending(a => a.StartDate).FirstOrDefault()?.StartDate;

                list.Add(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Phone,
                    timeSlot,
                    NextAppointment = nextAppointment,
                    LastAppointment = lastAppointment
                });
            }

            return new
            {
                List = list,
                Total = list.Count
            };
        }

        public async Task UpdateBranchForEmployee(Guid id, CreateEmployeeDto employee)
        {
            var existingEmp = await _context.Employees.FindAsync(id);
            if (existingEmp == null)
            {
                throw new InvalidOperationException("Employee not found or ID mismatch.");
            }
            else
            {
                existingEmp.BrId = employee.BrId;
                existingEmp.Salary = employee.Salary;

                _context.Employees.Update(existingEmp);
                await _context.SaveChangesAsync();
            }
        }

        public Employee CreateClinicTreatmentsForDentist(Guid dentId, List<int> clinicIds) 
        {
            //using(_context = new())
            //{
                Employee? employee = _context.Employees.Where(e => e.Id == dentId).FirstOrDefault();

                if (employee == null)
                {
                    return null;
                }

                foreach(int clinicTreatId in clinicIds)
                {
                    ClinicTreatment? clinic = _context.ClinicTreatments.Where(cl => cl.Id == clinicTreatId).FirstOrDefault();

                    if (clinic == null)
                    {
                        continue;
                    }
                    clinic.Dents.Add(employee);
                    _context.SaveChanges();
                }
                return employee;
            //}
        } 
    }
}
