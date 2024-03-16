using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ai_poker_coach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("testing");
        }
    }
}