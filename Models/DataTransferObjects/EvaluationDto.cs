using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class EvaluationDto : IHandStepDto
    {
        [Required]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }

        [Required]
        public string? Value { get; set; }

        public EvaluationDto() { }

        public EvaluationDto(Evaluation evaluation)
            : this()
        {
            Step = evaluation.Step;
            Player = evaluation.Player;
            Value = evaluation.Value;
        }
    }
}
