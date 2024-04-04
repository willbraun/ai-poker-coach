using System.ComponentModel.DataAnnotations;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandDto : IValidatableObject
    {
        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        public HandStepsDto? HandSteps { get; set; }

        [Required]
        public string? Analysis { get; set; }

        public HandDto() { }

        public HandDto(Hand hand)
            : this()
        {
            ApplicationUserId = hand.ApplicationUserId;
            HandSteps = new HandStepsDto(hand);
            Analysis = hand.Analysis;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = HandSteps!.Validate(new ValidationContext(HandSteps)).ToList();
            foreach (var validationResult in validationResults)
            {
                yield return new ValidationResult(validationResult.ErrorMessage, [nameof(validationResult.GetType)]);
            }
        }
    }
}
