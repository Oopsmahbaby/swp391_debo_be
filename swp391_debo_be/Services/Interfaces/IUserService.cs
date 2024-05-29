using swp391_debo_be.Constants;
using swp391_debo_be.Models;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IUserService
    {
        public bool IsRefreshToken(User user);
        public ApiRespone CreateUser(string email, string password);
    }
}
