using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public interface IHandStep
    {
        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; }
        public int Step { get; set; }
        public int Player { get; set; }
    }
}
