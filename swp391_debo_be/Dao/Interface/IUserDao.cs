using swp391_debo_be.Models;

namespace swp391_debo_be.Dao.Interface
{
    public interface IUserDao
    {
        public User GetUserById(Guid id);

        public User GetUserByEmail(string email);

        public User CreateUser(User user);

        public User UpdateUser(User user);

        public User DeleteUser(User user);

        public User GetUserByPhone(string phone);

        public string[] GetRolesName(User user);

        public bool IsRefreshTokenExist(User user);

    }
}
