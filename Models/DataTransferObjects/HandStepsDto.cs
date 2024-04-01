using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public class HandStepsDto : IValidatableObject
    {
        public string Name { get; set; } = "";

        [Required]
        [Range(0, 1, ErrorMessage = "Game style must be either 0 (tournament) or 1 (cash game).")]
        public int? GameStyle { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Player count must be a positive integer.")]
        public int? PlayerCount { get; set; }

        [Required]
        public int? Position { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Small blind must be positive.")]
        public decimal? SmallBlind { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Big blind must be positive.")]
        public decimal? BigBlind { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Ante must be positive.")]
        public decimal? Ante { get; set; } = 0;

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Big Blind Ante must be positive.")]
        public decimal? BigBlindAnte { get; set; } = 0;

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Stack must be positive.")]
        public decimal? MyStack { get; set; }

        public string? PlayerNotes { get; set; }

        [Required]
        public List<PotDto>? Pots { get; set; }

        [Required]
        public List<RoundDto>? Rounds { get; set; }

        public List<VillainDto> Villains { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Position > PlayerCount || Position < 1)
            {
                yield return new ValidationResult(
                    $"Position of {Position} is invalid. Must be in the range 1-{PlayerCount}.",
                    [nameof(Position)]
                );
            }

            if (Rounds!.Count < 1 || Rounds.Count > 4)
            {
                yield return new ValidationResult(
                    $"There must be between 1 and 4 rounds. Provided: {Rounds.Count}.",
                    [nameof(Rounds)]
                );
            }

            if (Rounds[0].Cards.Count != 2)
            {
                yield return new ValidationResult(
                    $"There must be 2 cards in the first round (hole cards). Provided: {Rounds[0].Cards.Count}.",
                    [nameof(Rounds)]
                );
            }

            if (Rounds[1].Cards.Count != 3)
            {
                yield return new ValidationResult(
                    $"There must be 3 cards in the second round (flop). Provided: {Rounds[1].Cards.Count}.",
                    [nameof(Rounds)]
                );
            }

            if (Rounds[2].Cards.Count != 1)
            {
                yield return new ValidationResult(
                    $"There must be 1 card in the third round (turn). Provided: {Rounds[2].Cards.Count}.",
                    [nameof(Rounds)]
                );
            }

            if (Rounds[3].Cards.Count != 1)
            {
                yield return new ValidationResult(
                    $"There must be 1 card in the fourth round (river). Provided: {Rounds[3].Cards.Count}.",
                    [nameof(Rounds)]
                );
            }

            if (Pots![0].PotIndex != 0)
            {
                yield return new ValidationResult($"Pots must start at 0. ", [nameof(Pots)]);
            }

            for (int i = 1; i < Pots!.Count; i++)
            {
                if (Pots[i].PotIndex - 1 != Pots[i - 1].PotIndex)
                {
                    yield return new ValidationResult(
                        $"Pots must increment by 1. Error at pot: {Pots[i].PotIndex}, expected {Pots[i - 1].PotIndex + 1}. ",
                        [nameof(Pots)]
                    );
                }
            }

            List<IHandStepDto> steps = [];
            foreach (var round in Rounds)
            {
                steps = [.. steps, .. round.Cards, round.Evaluation, .. round.Actions, .. round.PotActions];
            }

            foreach (var villain in Villains)
            {
                if (villain.Cards.Count != 2)
                {
                    yield return new ValidationResult(
                        $"Player {villain.Cards[0].Player} must have 2 cards. Provided: {villain.Cards.Count}.",
                        [nameof(Villains)]
                    );
                }

                steps = [.. steps, .. villain.Cards, villain.Evaluation];
            }

            if (steps[0].Step != 1)
            {
                yield return new ValidationResult($"Steps must start at 1. ", [nameof(Rounds)]);
            }

            int currentStep = 0;
            int playerMin;
            foreach (var step in steps)
            {
                List<ValidationResult> validationResults = [];
                bool isValid = Validator.TryValidateObject(step, new ValidationContext(step), validationResults, true);
                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                    {
                        yield return new ValidationResult(validationResult.ErrorMessage, [nameof(step)]);
                    }
                }

                if (step is PotActionDto potAction)
                {
                    if (potAction.PotIndex < 0 || potAction.PotIndex > Pots.Count - 1)
                    {
                        yield return new ValidationResult(
                            $"Pot index of {potAction.PotIndex} at step {potAction.Step} is outside the expected range of 0-{Pots.Count - 1}.",
                            [nameof(potAction)]
                        );
                    }
                }

                if (step is CardDto)
                {
                    playerMin = 0;
                }
                else
                {
                    playerMin = 1;
                }

                if (step.Player < playerMin || step.Player > PlayerCount)
                {
                    yield return new ValidationResult(
                        $"Player value of {step.Player} at step {step.Step} is invalid. Must be in range {playerMin}-{PlayerCount}.",
                        [nameof(step)]
                    );
                }

                if (step.Step - currentStep != 1)
                {
                    yield return new ValidationResult(
                        $"Steps must increment by 1. Error at step: {step.Step}, expected {currentStep + 1}. ",
                        [nameof(Rounds)]
                    );
                }
                currentStep++;
            }
        }
    }

    public class PotDto
    {
        public int PotIndex { get; set; }
        public string? Winner { get; set; }
    }

    public class RoundDto
    {
        public List<CardDto> Cards { get; set; } = [];
        public EvaluationDto Evaluation { get; set; } = new();
        public List<ActionDto> Actions { get; set; } = [];
        public List<PotActionDto> PotActions { get; set; } = [];
    }

    public class VillainDto
    {
        public List<CardDto> Cards { get; set; } = [];
        public EvaluationDto Evaluation { get; set; } = new();
    }
}
