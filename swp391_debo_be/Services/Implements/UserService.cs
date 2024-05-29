using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Helpers;
using swp391_debo_be.Models;
using swp391_debo_be.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace swp391_debo_be.Services.Implements
{
    public class UserService : IUserService
    {
        public ApiRespone CreateUser(string email, string password)
        {
            User user = new User()
            {
                Id = System.Guid.NewGuid(),
                Email = email,
                Password = Helper.HashPassword(password)
            };

            User createdUser = CUser.CreateUser(user);

            return new ApiRespone { Success = true, Data = createdUser , StatusCode = System.Net.HttpStatusCode.Created};
        }

        public bool IsRefreshToken(User user)
        {
            return CUser.IsRefreshTokenExist(user);
        }
    }
}
