using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;
using swp391_debo_be.Services.Interfaces;
using swp391_debo_be.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using swp391_debo_be.Constants;

namespace swp391_debo_be.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public AuthController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }


        [HttpPost("credentials/login")]
        public IActionResult LoginByCredentials([FromBody] UserRequestDto userRequest)
        {
            return Ok(tokenService.GenerateAccessToken(userRequest));
        }

        [HttpPost("google/login")]
        public async Task<IActionResult> LoginByGoogle([FromBody] GoogleAuthDto googleAuthDto)
        {
            var payload = await JwtProvider.VerifyGoogleTokenAsync(googleAuthDto);
            if (payload == null)
            {
                return BadRequest("Invalid External Authentication.");
            }

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
            new Claim(JwtRegisteredClaimNames.Email, payload.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = JwtProvider.GenerateToken(claims);
            return Ok(new ApiRespone{ Result = token });
        }
    }
}
