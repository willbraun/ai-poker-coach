using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public class EvaluationInputDto : IHandStepInputDto
    {
        [Required]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }
        
        [Required]
        public string? Value { get; set; }
    }
}