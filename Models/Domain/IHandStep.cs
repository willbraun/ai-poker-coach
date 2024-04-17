using System.ComponentModel.DataAnnotations.Schema;

namespace ai_poker_coach.Models.Domain
{
    public interface IHandStep
    {
        [ForeignKey("Hand")]
        public string HandId { get; set; }
        public Hand Hand { get; set; }
        public int Step { get; set; }
        public int Player { get; set; }
    }
}
