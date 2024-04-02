using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Action = ai_poker_coach.Models.Domain.Action;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class ActionDto : IHandStepDto
    {
        public ActionDto() { }

        public ActionDto(Action action)
            : this()
        {
            Step = action.Step;
            Player = action.Player;
            Decision = action.Decision;
            Bet = action.Bet;
        }

        [Required]
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