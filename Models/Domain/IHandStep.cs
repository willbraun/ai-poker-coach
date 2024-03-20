using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Domain
{
    public interface IHandStep
    {
        public int Step { get; set; }
        public int Player { get; set; }
    }
}