using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandInputDto : IValidatableObject
    {
        public string ApplicationUserId { get; set; } = "";
        public HandStepsDto HandSteps { get; set; } = new();
        public string Analysis { get; set; } = "";

        public HandInputDto() { }

        public HandInputDto(Hand hand)
        {
            ApplicationUserId = hand.ApplicationUserId;
            HandSteps = new HandStepsDto(hand);
            Analysis = hand.Analysis;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = HandSteps.Validate(new ValidationContext(HandSteps)).ToList();
            foreach (var validationResult in validationResults)
            {
                yield return new ValidationResult(validationResult.ErrorMessage, [nameof(validationResult.GetType)]);
            }
        }
    }
}
