using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Input;
using Microsoft.AspNetCore.Mvc;

namespace ai_poker_coach.Controllers
{
    [ApiController]
    [Route("[controller]/analyze")]
    public class HandController : ControllerBase
    {
        [HttpPost]
        public IActionResult Analyze([FromBody] AnalyzeInputDto requestBody)
        {
            return Ok(requestBody);
        }
    }
}