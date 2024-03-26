using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandDto : IValidatableObject
    {
        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        public HandStepsDto? HandStepsDto { get; set; }

        [Required]
        public string? Analysis { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = HandStepsDto!.Validate(new ValidationContext(HandStepsDto)).ToList();
            foreach (var validationResult in validationResults)
            {
                yield return new ValidationResult(validationResult.ErrorMessage, [nameof(HandStepsDto)]);
            }
        }
    }
}