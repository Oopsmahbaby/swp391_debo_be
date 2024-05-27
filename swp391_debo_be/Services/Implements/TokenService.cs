using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Models;
using swp391_debo_be.Services.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

namespace swp391_debo_be.Services.Implements
{
    public class TokenService : ITokenService
    {
        private const string UserNotFoundErrorMessage = "User not Found";

        public ApiRespone GenerateAccessToken(UserRequestDto user)
        {
            try
            {
                User foundUser = null;
                if (user.PhoneNumber == null && user.Email != null)
                {
                    foundUser = CUser.GetUserByEmail(user.Email);
                }

                if (user.PhoneNumber != null && user.Email == null)
                {
                    foundUser = CUser.GetUserByPhoneNumber(user.PhoneNumber);
                }

                if (foundUser == null)
                {
                    return new ApiRespone {StatusCode = System.Net.HttpStatusCode.NotFound ,ErrorMessage =  [ UserNotFoundErrorMessage ], IsSuccess = false };
                }

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, foundUser.Email),
                    new Claim(ClaimTypes.Name, foundUser.Username),
                    new Claim(ClaimTypes.MobilePhone, foundUser.Phone),
                };

                string accessToken = JwtProvider.GenerateToken(claims);

                string refreshToken = JwtProvider.GenerateRefreshToken(claims); 
                // Add code here to generate access token

                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Result = new { AccessToken = accessToken, RefreshToken = refreshToken } };
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void AddRole(User user, List<Claim> claims)
        {
            try
            {
                string[] roleName = CUser.GetRoleName(user);

                if (roleName != null && roleName.Length > 0)
                {
                    List<Claim> claimRoles = new List<Claim>();
                    foreach (var role in roleName)
                    {
                        claimRoles.Add(new Claim(ClaimTypes.Role, role));
                    }
                    claims.AddRange(claimRoles);
                }
            } catch (System.Exception)
            {
                throw;
            }
            
        }

        public ApiRespone GenerateRefreshToken(string token)
        {
            try
            {
                List<Claim> claims = JwtProvider.DecodeToken(token);
                Claim claim = claims.FirstOrDefault(x => x.Type == JwtConstant.KeyClaim.Email);
                User user = CUser.GetUserByEmail(claim.Value);

                if (CUser.IsRefreshTokenExist(user) == false)
                {
                    return new ApiRespone { StatusCode = System.Net.HttpStatusCode.Forbidden, ErrorMessage = new List<string> { UserNotFoundErrorMessage }, IsSuccess = false };
                }
                string accessToken = string.Empty;
                string refreshToken = string.Empty;
                
                JwtProvider.HandleRefreshToken(token, out accessToken, out refreshToken);

                var result = new { AccessToken = accessToken, RefreshToken = refreshToken };

                return new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Result = result };
            }
            catch (System.Exception)
            {
                throw;
            }
        }


    }
}
