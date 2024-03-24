using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text;
using static ai_poker_coach.Utils.PromptUtils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ai_poker_coach.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        private readonly IHttpClientFactory
        _httpClientFactory;

        public HandController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeInputDto requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string prompt = CreatePrompt(requestBody);

            var openaiBody = new
            {
                assistant_id = Environment.GetEnvironmentVariable("OPENAI_ASSISTANT_ID"),
                thread = new
                {
                    messages = new[] {
                        new {
                            role = "user",
                            content = prompt,
                            file_ids = new[] {
                                Environment.GetEnvironmentVariable("OPENAI_FILE_ID_1"),
                                Environment.GetEnvironmentVariable("OPENAI_FILE_ID_2"),
                                Environment.GetEnvironmentVariable("OPENAI_FILE_ID_3")
                            }
                        }
                    }
                },
                stream = true
            };

            string analysis = "";
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");
                httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
                var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/threads/runs", openaiBody);

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using StreamReader reader = new(stream, Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!;
                    if (line.Contains("\"object\":\"thread.message\",") &&
                        line.Contains("\"status\":\"completed\","))
                    {
                        analysis = line.Split("\"value\":")[1].Split(",\"annotations\":")[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while posting data to https://api.openai.com/v1/threads/runs : {ex.Message}, {ex.StackTrace}, {ex.Source}");
            }

            return Ok(analysis);
        }
    }
}