using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class CardDto : IHandStepDto
    {
        [Required]
        public int? Step { get; set; }
        
        [Required]
        public int? Player { get; set; }
        
        [Required]
        public string? Value { get; set; }
        
        [Required]
        public string? Suit { get; set; }
    }
}