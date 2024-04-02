using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.DataTransferObjects;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandDto : IValidatableObject
    {
        public HandDto() { }

        public HandDto(Hand hand)
            : this()
        {
            ApplicationUserId = hand.ApplicationUserId;
            HandSteps = new HandStepsDto(hand);
            Analysis = hand.Analysis;
        }

        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        public HandStepsDto? HandSteps { get; set; }

        [Required]
        public string? Analysis { get; set; }

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
