using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class PotActionDto : IHandStepDto
    {
        [Required]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Pot index must be positive.")]
        public int? PotIndex { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Bet must be positive.")]
        public decimal? Bet { get; set; }
    }
}