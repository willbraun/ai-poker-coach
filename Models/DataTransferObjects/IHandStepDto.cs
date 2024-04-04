using System.ComponentModel.DataAnnotations;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public interface IHandStepDto
    {
        [Required]
        public int? Step { get; set; }

        [Required]
        public int? Player { get; set; }
    }
}
