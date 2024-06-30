using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<ApiRespone> GenerateAccessToken(UserRequestDto user);

        public Task<ApiRespone> GenerateRefreshToken(TokenRequestDto token);

        public Task<ApiRespone> HandleLogout(string token);

        public string GetAuthorizationHeader(HttpRequest request);
        public string GetUserIdFromToken(string token);
    }
}
