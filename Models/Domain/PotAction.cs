using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class PotAction : IHandStep
    {
        [Key]
        public int PotActionId { get; set; }
        public int Player { get; set; }
        public int Bet { get; set; }
        
        [ForeignKey("Pot")]
        public int PotId { get; set; }
        public Pot Pot { get; set; } = new();

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
    }
}