using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class PotActionDto : IHandStepDto
    {
        public int Step { get; set; }
        public int Player { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Pot index must be positive.")]
        public int PotIndex { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Bet must be positive.")]
        public decimal Bet { get; set; }

        public PotActionDto() { }

        public PotActionDto(PotAction potAction)
        {
            Step = potAction.Step;
            Player = potAction.Player;
            PotIndex = potAction.Pot.PotIndex;
            Bet = potAction.Bet;
        }
    }
}
