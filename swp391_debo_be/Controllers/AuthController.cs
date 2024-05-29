﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using swp391_debo_be.Auth;
using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Services.Interfaces;


namespace swp391_debo_be.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        private readonly GoogleAuthSetting _googleAuthSetting;
        public AuthController(ITokenService tokenService, IOptions<GoogleAuthSetting> googleAuthSetting, IUserService userService)
        {
            this._tokenService = tokenService;
            this._googleAuthSetting = googleAuthSetting.Value;
            _userService = userService;
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("credentials/login")]
        public IActionResult LoginByCredentials([FromBody] UserRequestDto userRequest)
        {
            return Ok(_tokenService.GenerateAccessToken(userRequest));
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("google/login")]
        public async Task<IActionResult> LoginByGoogle([FromBody] GoogleAuthDto googleAuthDto)
        {
            var tokenResponse = await ExchangeCodeForTokenAsync(googleAuthDto.IdToken);

            if (tokenResponse == null)
            {
                return BadRequest("Invalid token");
            }

            return Ok(new ApiRespone { Data = tokenResponse });
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("google/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokenResponse = await RefreshTokenAsync(refreshToken);

            if (tokenResponse == null)
            {
                return BadRequest("Invalid token");
            }

            return Ok(new ApiRespone { Data = tokenResponse });
        }
        private async Task<TokenResponse> ExchangeCodeForTokenAsync(string code)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleAuthSetting.ClientId,
                    ClientSecret = _googleAuthSetting.ClientSecret
                }
            };

            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var tokenResponse = await flow.ExchangeCodeForTokenAsync("user-id", code, "postmessage", CancellationToken.None);

            return tokenResponse;
        }

        private async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleAuthSetting.ClientId,
                    ClientSecret = _googleAuthSetting.ClientSecret
                }
            };

            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var tokenResponse = await flow.RefreshTokenAsync("user-id", refreshToken, CancellationToken.None);

            return tokenResponse;
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUserDto createUserDto)
        {
            return Ok(_userService.CreateUser(createUserDto));
        }
    }
}
