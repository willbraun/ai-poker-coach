using System.ComponentModel.DataAnnotations;
using Action = ai_poker_coach.Models.Domain.Action;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class ActionDto : IHandStepDto
    {
        public int Step { get; set; }
        public int Player { get; set; }

        [Range(0, 6)]
        public int Decision { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Bet must be positive.")]
        public decimal Bet { get; set; }

        public ActionDto() { }

        public ActionDto(Action action)
        {
            Step = action.Step;
            Player = action.Player;
            Decision = action.Decision;
            Bet = action.Bet;
        }
    }
}
