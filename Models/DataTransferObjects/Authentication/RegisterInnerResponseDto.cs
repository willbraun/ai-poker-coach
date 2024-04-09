using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.DataTransferObjects.Authentication
{
    public class RegisterInnerResponseDto
    {
        public string type { get; set; } = "";
        public string title { get; set; } = "";
        public int status { get; set; }
        public Dictionary<string, string[]> errors { get; set; } = [];
    }
}
