using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Output
{
    public class ThreadResponseDto
    {
        public string Id { get; set; } = "";
        public string Object { get; set; } = "";
        public int Created_At { get; set; }
        public object MetaData { get; set; } = new { };
    }
}