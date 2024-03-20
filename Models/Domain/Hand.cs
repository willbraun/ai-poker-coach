using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.Domain
{
    public class Hand
    {
        [Key]
        public int HandId { get; set; }
        public string? Name { get; set; } = "";
        public int GameStyle { get; set; }
        public int PlayerCount { get; set; }
        public int Position { get; set; }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        public int BigBlindAnte { get; set; }
        public int MyStack { get; set; }
        public string? PlayerNotes { get; set; } = "";
        public ICollection<ActionGroup> ActionGroups { get; set; } = [];
        public ICollection<CardGroup> CardGroups { get; set; } = [];
        public string? Winners { get; set; } = "";
        public string? Analysis { get; set; } = "";
        public DateTime CreatedTime { get; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public Hand()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}