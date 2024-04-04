using Microsoft.AspNetCore.Identity;

namespace ai_poker_coach.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Hand> Hands { get; set; } = [];
    }
}
