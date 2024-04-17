using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandOutputDto
    {
        public string Id { get; set; } = "";
        public string ApplicationUserId { get; set; } = "";
        public HandStepsDto HandSteps { get; set; } = new();
        public string Analysis { get; set; } = "";
        public DateTime CreatedTime { get; set; }

        public HandOutputDto() { }

        public HandOutputDto(Hand hand)
            : this()
        {
            Id = hand.Id;
            ApplicationUserId = hand.ApplicationUserId;
            HandSteps = new HandStepsDto(hand);
            Analysis = hand.Analysis;
            CreatedTime = hand.CreatedTime;
        }
    }
}
