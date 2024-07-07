using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Card : IHandStep
    {
        [Key]
        public int CardId { get; set; }
        public string Value { get; set; } = "";
        public string Suit { get; set; } = "";

        [ForeignKey("Hand")]
        public string HandId { get; set; } = "";
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }

        public Card() { }

        public Card(Hand hand, CardDto cardDto)
        {
            Hand = hand;
            HandId = hand.Id;
            Step = cardDto.Step;
            Player = cardDto.Player;
            Value = cardDto.Value;
            Suit = cardDto.Suit;
        }
    }
}
