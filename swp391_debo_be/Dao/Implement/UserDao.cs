using swp391_debo_be.Dao.Interface;
using swp391_debo_be.DBContext;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Helpers;

namespace swp391_debo_be.Dao.Implement
{
    public class UserDao : IUserDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context (new Microsoft.EntityFrameworkCore.DbContextOptions<DeboDev02Context>());

        public UserDao()
        {
        }

        public UserDao(DeboDev02Context context)
        {
            this._context = context;
        }

        public User CreateUser(User user)
        { 
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

            return user;
        }

        public string[] GetRolesName(User user)
        {
            int? roleId = user.Role;

            Role? role = _context.Roles.Find(roleId);

            if (role == null)
            {
                return new string[0];
            }

            return new string[] { role.Role1 };
        }

        public User GetUserByEmail(string email)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }

        public User GetUserById(Guid id)
        {
            User? user = _context.Users.Find(id);
            return user;
        }

        public User GetUserByPhone(string phone)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Phone == phone);

            return user;
        }

        public bool IsRefreshTokenExist(User user)
        {
            User foundUser = GetUserById(user.Id);

            if (foundUser.RefreshToken == null)
            {
                return false;
            }

            return true;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList<User>();
        }


        public bool SaveRefreshToken(Guid userId, string refreshToken)
        {
            User? user = GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            user.RefreshToken = refreshToken;
            UpdateUser(user);
            return true;
        }

        public bool DeleteRefreshToken(Guid userId)
        {
            User? user = GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            user.RefreshToken = null;

            UpdateUser(user);

            return true;
        }

        public bool IsPasswordExist(string password, User user)
        {
            User? foundUser = GetUserById(user.Id);

            if (foundUser.Password == password)
            {
                return true;
            }

            return false;
        }

        public async Task<Guid> CreateNewStaff(EmployeeDto employee)
        {
            var newStaff = new User
            {
                Id = new Guid(Guid.NewGuid().ToString()),
                Role = 3,
                Username = employee.Username,
                Email = employee.Email,
                Password = HashPasswordHelper.HashPassword(employee.Password),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Phone = employee.Phone,
                Address = employee.Address,
                DateOfBirthday = employee.DateOfBirthday,
                MedRec = employee.MedRec,
                Avt = employee.Avt,
            };
            _context.Users.Add(newStaff);
            await _context.SaveChangesAsync();
            return newStaff.Id;
        }

        public async Task<Guid> CreateNewDentist(EmployeeDto employee)
        {
            var newDent = new User
            {
                Id = new Guid(Guid.NewGuid().ToString()),
                Role = 4,
                Username = employee.Username,
                Email = employee.Email,
                Password = HashPasswordHelper.HashPassword(employee.Password),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Phone = employee.Phone,
                Address = employee.Address,
                DateOfBirthday = employee.DateOfBirthday,
                MedRec = employee.MedRec,
                Avt = employee.Avt,
            };
            _context.Users.Add(newDent);
            await _context.SaveChangesAsync();
            return newDent.Id;
        }

        public async Task<Guid> CreateNewManager(EmployeeDto employee)
        {
            var newManager = new User
            {
                Id = new Guid(Guid.NewGuid().ToString()),
                Role = 2,
                Username = employee.Username,
                Email = employee.Email,
                Password = HashPasswordHelper.HashPassword(employee.Password),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Phone = employee.Phone,
                Address = employee.Address,
                DateOfBirthday = employee.DateOfBirthday,
                MedRec = employee.MedRec,
                Avt = employee.Avt,
            };
            _context.Users.Add(newManager);
            await _context.SaveChangesAsync();
            return newManager.Id;
        }
    }
}
