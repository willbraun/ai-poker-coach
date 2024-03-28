using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class ActionDto : IHandStepDto
    {
        [Required]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }

        [Required]
        [Range(0, 5)]
        public int? Decision { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Bet must be positive.")]
        public decimal? Bet { get; set; }

        public List<PotActionDto> PotActions { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PotActions == null)
            {
                yield break;
            }

            foreach (var potAction in PotActions)
            {
                List<ValidationResult> validationResults = [];
                bool isValid = Validator.TryValidateObject(potAction, new ValidationContext(potAction), validationResults, true);
                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                    {
                        yield return new ValidationResult(validationResult.ErrorMessage, [nameof(potAction)]);
                    }
                }
            }
        }
    }
}