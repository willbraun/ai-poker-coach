using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public class ActionInputDto : IHandStepInputDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Step must be a positive integer.")]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }

        [Required]
        [Range(0, 5)]
        public int? Decision { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Bet must be positive.")]
        public decimal? Bet { get; set; }
    }
}