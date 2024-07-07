using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class CardDto : IHandStepDto
    {
        public int Step { get; set; }
        public int Player { get; set; }
        public string Value { get; set; } = "";
        public string Suit { get; set; } = "";

        public CardDto() { }

        public CardDto(Card card)
        {
            Step = card.Step;
            Player = card.Player;
            Value = card.Value;
            Suit = card.Suit;
        }
    }
}
