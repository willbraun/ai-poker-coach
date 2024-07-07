using System.ComponentModel.DataAnnotations;

namespace ai_poker_coach.Models.DataTransferObjects
{
    public interface IHandStepDto
    {
        public int Step { get; set; }
        public int Player { get; set; }
    }
}
