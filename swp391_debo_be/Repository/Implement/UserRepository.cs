using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDao _userDao;

        public UserRepository()
        {
            this._userDao = new UserDao();
        }

        public User CreateUser(User user)
        {
            return _userDao.CreateUser(user);
        }

        public User DeleteUser(User user)
        {
            return _userDao.DeleteUser(user);
        }

        public string[] getRolesName(User user)
        {
            return _userDao.GetRolesName(user);
        }

        public User GetUserByEmail(string email)
        {
            return _userDao.GetUserByEmail(email);
        }

        public User GetUserById(Guid id)
        {
            return _userDao.GetUserById(id);
        }

        public User GetUserByPhone(string phone)
        {
            return _userDao.GetUserByPhone(phone);
        }

        public List<User> GetUsers()
        {
            return _userDao.GetUsers();
        }

        public bool IsRefreshTokenExist(User user)
        {
            return _userDao.IsRefreshTokenExist(user);

        }

        public bool SaveRefreshToken(Guid userId, string refreshToken)
        {
            return _userDao.SaveRefreshToken(userId, refreshToken);
        }

        public User UpdateUser(User user)
        {
            return _userDao.UpdateUser(user);
        }

        public bool DeleteRefreshToken(Guid userId)
        {
            return _userDao.DeleteRefreshToken(userId);
        }

        public bool IsPasswordExist(User user)
        {
            return _userDao.IsPasswordExist(user);
        }
    }
}
