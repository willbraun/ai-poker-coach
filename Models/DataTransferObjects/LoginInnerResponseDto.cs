namespace ai_poker_coach.Models.DataTransferObjects
{
    public class LoginInnerResponseDto
    {
        public string tokenType { get; set; } = "";
        public string accessToken { get; set; } = "";
        public int expiresIn { get; set; } = 0;
        public string refreshToken { get; set; } = "";
    }
}
