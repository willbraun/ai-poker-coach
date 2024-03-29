using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class Action : IHandStep
    {
        [Key]
        public int ActionId { get; set; }
        public int Decision { get; set; }
        public decimal Bet { get; set; }
        public decimal Pot { get; set; }

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }
    }
}