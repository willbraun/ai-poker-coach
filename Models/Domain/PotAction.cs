using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class PotAction : IHandStep
    {
        [Key]
        public int PotActionId { get; set; }
        public int Player { get; set; }
        public decimal Bet { get; set; }

        [ForeignKey("Pot")]
        public int PotId { get; set; }
        public Pot Pot { get; set; } = new();

        [ForeignKey("Hand")]
        public string HandId { get; set; } = "";
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }

        public PotAction() { }

        public PotAction(Hand hand, PotActionDto potActionDto)
        {
            var pot = hand.Pots.First(pot => pot.PotIndex == potActionDto.PotIndex);

            Hand = hand;
            HandId = hand.Id;
            Step = potActionDto.Step;
            Player = potActionDto.Player;
            Pot = pot;
            PotId = pot.PotId;
            Bet = potActionDto.Bet;
        }
    }
}
