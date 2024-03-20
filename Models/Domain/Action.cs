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
        public int Bet { get; set; }

        [ForeignKey("ActionGroup")]
        public int ActionGroupId { get; set; }
        public ActionGroup ActionGroup { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }
    }
}