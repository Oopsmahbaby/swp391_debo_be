using Microsoft.Identity.Client;

namespace swp391_debo_be.Dto.Implement
{
    public class TokenRequestDto
    {
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }
    }
}
