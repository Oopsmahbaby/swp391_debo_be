using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Helpers;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace swp391_debo_be.Services.Implements
{
    public class UserService : IUserService
    {
        public ApiRespone CreateUser(CreateUserDto createUserDto)
        {

            if (CUser.GetUserByEmail(createUserDto.Email) != null)
            {
                return new ApiRespone { Success = false, Message = "Email is already exist", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }

            User user = new User()
            {
                Id = System.Guid.NewGuid(),
                Email = createUserDto.Email,
                // Create phone number for user
                Phone = createUserDto.PhoneNumber, 
                // Role = 5 la Customer -> dua tren database moi
                Role = 5,
                Password = Helper.HashPassword(createUserDto.password)
            };

            User createdUser = CUser.CreateUser(user);

            return new ApiRespone { Success = true, Data = createdUser , StatusCode = System.Net.HttpStatusCode.Created};
        }

        public object? GetProfile(string? userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            return CUser.GetUsers();
        }

        public bool IsRefreshToken(User user)
        {
            return CUser.IsRefreshTokenExist(user);
        }
    }
}
