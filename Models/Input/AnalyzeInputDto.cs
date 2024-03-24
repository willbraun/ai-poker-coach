using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public class AnalyzeInputDto : IValidatableObject
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
        public decimal Ante { get; set; } = 0;

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Big Blind Ante must be positive.")]
        public decimal BigBlindAnte { get; set; } = 0;

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Stack must be positive.")]
        public decimal? MyStack { get; set; }

        [Required]
        public string? PlayerNotes { get; set; }

        [Required]
        public string? Winners { get; set; }

        [Required]
        public List<RoundInputDto>? Rounds { get; set; }

        public List<VillainInputDto> Villains { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Position > PlayerCount || Position < 1)
            {
                yield return new ValidationResult($"Position of {Position} is invalid. Must be in the range 1-{PlayerCount}.", [nameof(Position)]);
            }

            if (Rounds!.Count < 1 || Rounds.Count > 4)
            {
                yield return new ValidationResult($"There must be between 1 and 4 rounds. Provided: {Rounds.Count}.", [nameof(Rounds)]);
            }

            if (Rounds[0].Cards.Count != 2)
            {
                yield return new ValidationResult($"There must be 2 cards in the first round (hole cards). Provided: {Rounds[0].Cards.Count}.", [nameof(Rounds)]);
            }

            if (Rounds[1].Cards.Count != 3)
            {
                yield return new ValidationResult($"There must be 3 cards in the second round (flop). Provided: {Rounds[1].Cards.Count}.", [nameof(Rounds)]);
            }

            if (Rounds[2].Cards.Count != 1)
            {
                yield return new ValidationResult($"There must be 1 card in the third round (turn). Provided: {Rounds[2].Cards.Count}.", [nameof(Rounds)]);
            }

            if (Rounds[3].Cards.Count != 1)
            {
                yield return new ValidationResult($"There must be 1 card in the fourth round (river). Provided: {Rounds[3].Cards.Count}.", [nameof(Rounds)]);
            }

            List<IHandStepInputDto> steps = [];
            foreach (var round in Rounds)
            {
                steps = [.. steps, .. round.Cards, round.Evaluation, .. round.Actions];
            }

            foreach (var villain in Villains)
            {
                if (villain.Cards.Count != 2)
                {
                    yield return new ValidationResult($"Player {villain.Cards[0].Player} must have 2 cards. Provided: {villain.Cards.Count}.", [nameof(Villains)]);
                }

                steps = [.. steps, .. villain.Cards, villain.Evaluation];
            }

            if (steps[0].Step != 1)
            {
                yield return new ValidationResult($"Steps must start at 1. ", [nameof(Rounds)]);
            }

            int current = 0;
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

                if (step is CardInputDto)
                {
                    playerMin = 0;
                }
                else
                {
                    playerMin = 1;
                }

                if (step.Player < playerMin || step.Player > PlayerCount)
                {
                    yield return new ValidationResult($"Player value of {step.Player} at step {step.Step} is invalid. Must be in range {playerMin}-{PlayerCount}.", [nameof(step)]);
                }

                if (step.Step - current != 1)
                {
                    yield return new ValidationResult($"Steps must increment by 1. Error at step: {step.Step}, expected {current + 1}. ", [nameof(Rounds)]);
                }
                current++;
            }
        }
    }

    public class RoundInputDto
    {
        public List<CardInputDto> Cards { get; set; } = [];
        public EvaluationInputDto Evaluation { get; set; } = new();
        public List<ActionInputDto> Actions { get; set; } = [];
    }

    public class VillainInputDto
    {
        public List<CardInputDto> Cards { get; set; } = [];
        public EvaluationInputDto Evaluation { get; set; } = new();
    }
}