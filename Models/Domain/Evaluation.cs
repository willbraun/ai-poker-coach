using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Evaluation : IHandStep
    {
        [Key]
        public int EvaluationId { get; set; }
        public string Value { get; set; } = "";

        [ForeignKey("Hand")]
        public string HandId { get; set; } = "";
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }

        public Evaluation() { }

        public Evaluation(Hand hand, EvaluationDto evalutationDto)
        {
            Hand = hand;
            HandId = hand.Id;
            Step = evalutationDto.Step;
            Player = evalutationDto.Player;
            Value = evalutationDto.Value;
        }
    }
}
