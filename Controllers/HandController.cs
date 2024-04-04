using System.Text;
using ai_poker_coach.Models.DataTransferObjects;
using ai_poker_coach.Models.Domain;
using DotNet8Authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using static ai_poker_coach.Utils.PromptUtils;

namespace ai_poker_coach.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IdentityDataContext _dbContext;

        public HandController(IHttpClientFactory httpClientFactory, IdentityDataContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] HandStepsDto body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string prompt = CreatePrompt(body);

            var openaiBody = new
            {
                assistant_id = Environment.GetEnvironmentVariable("OPENAI_ASSISTANT_ID"),
                thread = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = prompt,
                            file_ids = new[]
                            {
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
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Authorization,
                    $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}"
                );
                httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
                var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/threads/runs", openaiBody);

                response.EnsureSuccessStatusCode();

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using StreamReader reader = new(stream, Encoding.UTF8);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!;
                    if (line.Contains("\"object\":\"thread.message\",") && line.Contains("\"status\":\"completed\","))
                    {
                        analysis = line.Split("\"value\":")[1].Split(",\"annotations\":")[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while analyzing data: {ex.Message}"
                );
            }

            return Ok(analysis);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HandDto body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == body.ApplicationUserId);
            if (user == null)
            {
                return NotFound($"User ID of {body.ApplicationUserId} does not exist.");
            }

            user.Hands.Add(new Hand(user, body));

            try
            {
                int rowsAffected = await _dbContext.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok("Successfully added hand");
                }
                else
                {
                    return BadRequest("Failed to add hand");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? userId = null)
        {
            try
            {
                IQueryable<Hand> query = _dbContext
                    .Hands.Include(h => h.Pots)
                    .Include(h => h.Cards)
                    .Include(h => h.Evaluations)
                    .Include(h => h.Actions)
                    .Include(h => h.PotActions)
                    .OrderByDescending(h => h.CreatedTime);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user == null)
                    {
                        return NotFound($"User ID of \"{userId}\" does not exist.");
                    }

                    query = query.Where(hand => hand.ApplicationUserId == userId);
                }

                List<Hand> hands = await query.ToListAsync();
                List<HandDto> handDtos = hands.Select(hand => new HandDto(hand)).ToList();

                return Ok(handDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int handId)
        {
            if (handId == 0)
            {
                return BadRequest("Hand ID is required.");
            }

            var hand = await _dbContext.Hands.FirstOrDefaultAsync(h => h.HandId == handId);
            if (hand == null)
            {
                return NotFound($"Hand ID of \"{handId}\" does not exist.");
            }

            var user = await _dbContext
                .ApplicationUsers.Include(u => u.Hands)
                .FirstOrDefaultAsync(u => u.Hands.Any(h => h.HandId == handId));
            if (user == null)
            {
                return NotFound($"No user associated with hand ID of \"{handId}\".");
            }

            user.Hands.Remove(hand);

            try
            {
                int rowsAffected = await _dbContext.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok("Successfully deleted hand");
                }
                else
                {
                    return BadRequest("Failed to delete hand");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error occurred: {ex.Message}");
            }
        }
    }
}
