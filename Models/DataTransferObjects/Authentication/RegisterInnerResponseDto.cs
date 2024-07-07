namespace ai_poker_coach.Models.DataTransferObjects.Authentication
{
    public class RegisterInnerResponseDto
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = [];
    }
}
