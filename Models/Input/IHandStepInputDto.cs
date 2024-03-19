using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public interface IHandStepInputDto
    {
        public int Step { get; set; }
        public int Player { get; set; }
    }
}