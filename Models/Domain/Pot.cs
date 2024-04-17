using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Pot
    {
        [Key]
        public int PotId { get; set; }
        public int PotIndex { get; set; }
        public string Winner { get; set; } = "";
        public ICollection<PotAction> PotActions = [];

        [ForeignKey("Hand")]
        public string HandId { get; set; } = "";
        public Hand Hand { get; set; } = new();

        public Pot() { }

        public Pot(Hand hand, PotDto potDto)
        {
            Hand = hand;
            HandId = hand.Id;
            PotIndex = potDto.PotIndex;
            Winner = potDto.Winner!;
        }
    }
}
