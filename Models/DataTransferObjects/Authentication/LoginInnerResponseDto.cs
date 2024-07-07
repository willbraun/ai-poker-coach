namespace ai_poker_coach.Models.DataTransferObjects.Authentication
{
    public class LoginInnerResponseDto
    {
        public string TokenType { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public int ExpiresIn { get; set; } = 0;
        public string RefreshToken { get; set; } = "";
    }
}
