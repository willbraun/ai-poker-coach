using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public class Hand
    {
        [Key]
        public int HandId { get; set; }
        public string Name { get; set; } = "";
        public int GameStyle { get; set; }
        public int PlayerCount { get; set; }
        public int Position { get; set; }
        public decimal SmallBlind { get; set; }
        public decimal BigBlind { get; set; }
        public decimal Ante { get; set; }
        public decimal BigBlindAnte { get; set; }
        public decimal MyStack { get; set; }
        public string PlayerNotes { get; set; } = "";
        public ICollection<Pot> Pots { get; set; } = [];
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<Evaluation> Evaluations { get; set; } = [];
        public ICollection<Action> Actions { get; set; } = [];
        public ICollection<PotAction> PotActions { get; set; } = [];
        public string Analysis { get; set; } = "";
        public DateTime CreatedTime { get; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = "";
        public ApplicationUser ApplicationUser { get; set; } = new();

        public Hand()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}