using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private readonly IConfiguration configuration;

        public AuthController(ITokenService tokenService, IConfiguration configuration, IUserService userService)
        {
            this._tokenService = tokenService;
            this.configuration = configuration;
            this._userService = userService;
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("google/login")]
        public async Task<IActionResult> LoginByGoogle([FromQuery] string code)
        {
            var userInfo = await ExchangeCodeForTokenAsync(code);

            if (!_userService.ValidAdminEmail(userInfo.Email))
            {
                return Ok(new ApiRespone { StatusCode = System.Net.HttpStatusCode.NotFound, Message = "Invalid Email", Success = false });
            }

            return Ok(new ApiRespone { StatusCode = System.Net.HttpStatusCode.OK, Message = "Valid accesss", Success = true, Data = userInfo });
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

        private async Task<UserInfoGoogle> ExchangeCodeForTokenAsync(string code)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = configuration["Authentication:Google:ClientId"],
                    ClientSecret = configuration["Authentication:Google:ClientSecret"]
                }
            };
            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var tokenResponse = await flow.ExchangeCodeForTokenAsync("user-id", code, "http://localhost:5173", CancellationToken.None);

            if (tokenResponse != null)
            {
                // Get the user's profile information
                var userInfo = await GetUserInfoAsync(tokenResponse.AccessToken);
                return userInfo;
            }

            return null;
        }
        private async Task<UserInfoGoogle> GetUserInfoAsync(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo");

                if (!string.IsNullOrEmpty(response))
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfoGoogle>(response);
                    return userInfo;
                }
            }

            return null;
        }

        private async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = configuration["Authentication:Google:ClientId"],
                    ClientSecret = configuration["Authentication:Google:ClientSecret"]
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

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("credentials/refreshToken")]
        public IActionResult RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
        {
            return Ok(_tokenService.GenerateRefreshToken(tokenRequestDto));
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string token)
        {
            return Ok(_tokenService.HandleLogout(token));
        }

    }

}
