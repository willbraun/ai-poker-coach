using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Data
{
    public class Action
    {
        public int ActionId { get; set; }


        //         Id
        // HandId
        // HandStep
        // Player(1-9, by position to SB)
        // Action(fold, check, call, raise), maybe enumerated to 0, 1, 2, 3
        // Bet

    }
}