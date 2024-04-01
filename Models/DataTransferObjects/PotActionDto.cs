using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class PotActionDto : IHandStepDto
    {
        public PotActionDto() { }

        public PotActionDto(PotAction potAction)
            : this()
        {
            Step = potAction.Step;
            Player = potAction.Player;
            PotIndex = potAction.Pot.PotIndex;
            Bet = potAction.Bet;
        }

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
