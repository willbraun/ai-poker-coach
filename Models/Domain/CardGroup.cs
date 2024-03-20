using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class CardGroup
    {
        [Key]
        public int CardGroupId { get; set; }
        public int GroupOrder { get; set; }
        public ICollection<Card> Cards { get; set; } = [];
        public string Evaluation { get; set; } = "";

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
    }
}