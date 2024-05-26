using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Models;

namespace swp391_debo_be.Services.Interfaces
{
    public interface ITokenService
    {
        public ApiRespone GenerateAccessToken(UserRequestDto user);

        public ApiRespone GenerateRefreshToken(string token);
    }
}
