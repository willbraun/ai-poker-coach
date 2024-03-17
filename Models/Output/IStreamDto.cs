using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Any;

namespace ai_poker_coach.Models.Output
{
    public interface IStreamDto
    {
        public AnyType? Data { get; set; }
        public string Event { get; set; }
    }
}