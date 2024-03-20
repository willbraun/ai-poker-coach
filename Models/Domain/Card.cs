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

        [ForeignKey("CardGroup")]
        public int CardGroupId { get; set; }
        public CardGroup CardGroup { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }
    }
}