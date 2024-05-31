using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace swp391_debo_be.Auth
{
    public static class JwtProvider
    {
        private static readonly IConfiguration _configuration;

        static JwtProvider()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="claims">the</param>
        /// <returns></returns>
        public static string GenerateToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(JwtSettingModel.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(JwtSettingModel.ExpireDayAccessToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(JwtSettingModel.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(JwtSettingModel.ExpireDayRefreshToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public static void HandleRefreshToken(string tokenInput, out string accessToken, out string refreshToken)
        {
            List<Claim> claims = DecodeToken(tokenInput);

            // Generate access token
            accessToken = GenerateToken(claims);

            // Generate refresh token
            refreshToken = GenerateToken(claims);
        }

        public static List<Claim> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JwtSettingModel.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var claims = jwtToken.Claims.ToList();

            return claims;
        }

        public static bool Validation(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettingModel.SecretKey);


            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            return true;
        }

        public static async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(GoogleAuthDto googleAuthDto)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { _configuration["GoogleAuthSettings:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleAuthDto.IdToken, settings);

                return payload;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
