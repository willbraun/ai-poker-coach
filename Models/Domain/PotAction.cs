using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class PotAction
    {
        [Key]
        public int PotActionId { get; set; }
        public int Bet { get; set; }

        [ForeignKey("Action")]
        public int ActionId { get; set; }
        public Action Action { get; set; } = new();
        
        [ForeignKey("Pot")]
        public int PotId { get; set; }
        public Pot Pot { get; set; } = new();
    }
}