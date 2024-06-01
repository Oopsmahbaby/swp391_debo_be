using swp391_debo_be.Dao.Interface;
using swp391_debo_be.DBContext;
using swp391_debo_be.Entity.Implement;

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

            if(user == null)
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
    }
}
