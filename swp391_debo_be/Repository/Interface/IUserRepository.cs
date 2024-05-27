using Microsoft.Owin.Security;
using swp391_debo_be.Models;

namespace swp391_debo_be.Repository.Interface
{
    public interface IUserRepository
    {
        public User GetUserById(Guid id);

        public User GetUserByEmail(string email);

        public User CreateUser(User user);

        public User UpdateUser(User user);

        public User DeleteUser(User user);

        public User GetUserByPhone(string phone);

        public string[] getRolesName(User user);

        public bool IsRefreshTokenExist(User user);
    }
}
