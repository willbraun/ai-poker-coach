using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Text.Json;
using ai_poker_coach.Models.Output;

namespace ai_poker_coach.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HandController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeInputDto requestBody)
        {

            // create prompt string from requestBody
            string message = "Give me some poker tips for playing pocket 8s";

            var openaiBody = new {
                assistant_id = Environment.GetEnvironmentVariable("OPENAI_ASSISTANT_ID"),
                thread = new {
                    messages = new[] {
                        new {
                            role = "user",
                            content = message,
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

            // var openaiBodyString = JsonSerializer.Serialize(openaiBody);

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                // httpClient.DefaultRequestHeaders.Add(HeaderNames.ContentType, "application/json");
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");
                httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
                var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/threads/runs", openaiBody);
                
                using Stream stream = await response.Content.ReadAsStreamAsync();
                using StreamReader reader = new(stream);
                string analysis = "";
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!;
                    

                    Console.WriteLine(line);
                    var json = JsonSerializer.Deserialize<IStreamDto>(line);
                    Console.WriteLine(json);
                    if (json is MessageDeltaDto messageDelta)
                    {
                        analysis += messageDelta.Data.Delta.Content[0].Text.Value;
                    }
                }

                Console.WriteLine($"Analysis: {analysis}");
            
                

                // ThreadResponseDto data = JsonSerializer.Deserialize<ThreadResponseDto>(dataString)!;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while posting data to https://api.openai.com/v1/threads/runs : {ex.Message}, {ex.StackTrace}, {ex.Source}");
            }

            // return response

            return Ok("Successful - testing");
        }
    }
}