using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class Card : IHandStep
    {
        [Key]
        public int CardId { get; set; }
        public string Value { get; set; } = "";
        public string Suit { get; set; } = "";

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }
    }
}