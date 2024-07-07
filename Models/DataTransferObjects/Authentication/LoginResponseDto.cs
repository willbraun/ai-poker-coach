using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects.Authentication
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public int ExpiresIn { get; set; } = 0;
        public string RefreshToken { get; set; } = "";

        public LoginResponseDto(ApplicationUser user, LoginInnerResponseDto loginInnerResponseDto)
        {
            UserId = user.Id;
            AccessToken = loginInnerResponseDto.AccessToken;
            ExpiresIn = loginInnerResponseDto.ExpiresIn;
            RefreshToken = loginInnerResponseDto.RefreshToken;
        }
    }
}
