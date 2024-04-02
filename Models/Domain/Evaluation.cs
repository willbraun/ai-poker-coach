using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Evaluation : IHandStep
    {
        [Key]
        public int EvaluationId { get; set; }
        public string Value { get; set; } = "";

        [ForeignKey("Hand")]
        public int HandId { get; set; }
        public Hand Hand { get; set; } = new();
        public int Step { get; set; }
        public int Player { get; set; }

        public Evaluation() { }

        public Evaluation(Hand hand, EvaluationDto evalutationDto)
        {
            Hand = hand;
            HandId = hand.HandId;
            Step = evalutationDto.Step ?? 0;
            Player = evalutationDto.Player ?? 0;
            Value = evalutationDto.Value!;
        }
    }
}
