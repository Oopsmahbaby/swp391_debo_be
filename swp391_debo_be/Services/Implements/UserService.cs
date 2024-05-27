using swp391_debo_be.Cores;
using swp391_debo_be.Models;
using swp391_debo_be.Services.Interfaces;

namespace swp391_debo_be.Services.Implements
{
    public class UserService : IUserService
    {
        public bool IsRefreshToken(User user)
        {
            return CUser.IsRefreshTokenExist(user);
        }
    }
}
