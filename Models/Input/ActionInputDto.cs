using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public class ActionInputDto : IHandStepInputDto
    {
        public int Step { get; set; }
        public int Player { get; set; }
        public int Decision { get; set; }
        public int Bet { get; set; }
    }
}