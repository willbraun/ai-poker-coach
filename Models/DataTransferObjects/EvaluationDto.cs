using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class EvaluationDto : IHandStepDto
    {
        public int Step { get; set; }
        public int Player { get; set; }
        public string Value { get; set; } = "";

        public EvaluationDto() { }

        public EvaluationDto(Evaluation evaluation)
        {
            Step = evaluation.Step;
            Player = evaluation.Player;
            Value = evaluation.Value;
        }
    }
}
