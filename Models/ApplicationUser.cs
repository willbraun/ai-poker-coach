using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ai_poker_coach.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Hand>? Hands { get; set; }
    }
}