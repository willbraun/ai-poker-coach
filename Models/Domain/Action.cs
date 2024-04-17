using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Action : IHandStep
    {
        [Key]
        public int ActionId { get; set; }
        public int Decision { get; set; }
        public decimal Bet { get; set; }

        [ForeignKey("Hand")]
        public string HandId { get; set; } = "";
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }

        public Action() { }

        public Action(Hand hand, ActionDto actionDto)
        {
            Hand = hand;
            HandId = hand.Id;
            Step = actionDto.Step ?? 0;
            Player = actionDto.Player ?? 0;
            Decision = actionDto.Decision ?? 0;
            Bet = actionDto.Bet ?? 0;
        }
    }
}
