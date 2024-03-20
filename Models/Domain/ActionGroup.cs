using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class ActionGroup
    {
        [Key]
        public int ActionGroupId { get; set; }
        public int GroupOrder { get; set; }
        public ICollection<Action> Actions { get; set; } = [];

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
    }
}