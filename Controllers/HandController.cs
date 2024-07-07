using System.Security.Claims;
using ai_poker_coach.Models.DataTransferObjects;
using ai_poker_coach.Models.Domain;
using DotNet8Authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ai_poker_coach.Utils.PromptUtils;

namespace ai_poker_coach.Controllers
{
    public class ErrorMessage(string message)
    {
        public string Message { get; set; } = message;
    }

    [ApiController]
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
        [Authorize]
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

            string analysis =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nVivamus lacinia odio vitae vestibulum vestibulum.\nSed ac felis sit amet ligula pharetra condimentum.\nMorbi in sem quis dui placerat ornare.\nPellentesque odio nisi, euismod in, pharetra a, ultricies in, diam.\nSed arcu. Cras consequat.\nLorem ipsum dolor sit amet, consectetur adipiscing elit.\nVivamus lacinia odio vitae vestibulum vestibulum.\nSed ac felis sit amet ligula pharetra condimentum.\nMorbi in sem quis dui placerat ornare.\nPellentesque odio nisi, euismod in, pharetra a, ultricies in, diam.\nSed arcu. Cras consequat.";
            await Task.Delay(3000);
            // try
            // {
            //     var httpClient = _httpClientFactory.CreateClient();
            //     httpClient.DefaultRequestHeaders.Add(
            //         HeaderNames.Authorization,
            //         $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}"
            //     );
            //     httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
            //     var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/threads/runs", openaiBody);

            //     response.EnsureSuccessStatusCode();

            //     using Stream stream = await response.Content.ReadAsStreamAsync();
            //     using StreamReader reader = new(stream, Encoding.UTF8);
            //     while (!reader.EndOfStream)
            //     {
            //         string line = reader.ReadLine()!;
            //         if (line.Contains("\"object\":\"thread.message\",") && line.Contains("\"status\":\"completed\","))
            //         {
            //             analysis = line.Split("\"value\":")[1].Split(",\"annotations\":")[0];
            //             break;
            //         }
            //     }
            // }
            // catch (Exception ex)
            // {
            //     return StatusCode(
            //         StatusCodes.Status500InternalServerError,
            //         $"An error occurred while analyzing data: {ex.Message}"
            //     );
            // }

            var result = new { Analysis = analysis };

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] HandInputDto body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == body.ApplicationUserId);
            if (user == null)
            {
                return NotFound(new ErrorMessage($"User ID of {body.ApplicationUserId} does not exist."));
            }

            string? authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId != user.Id)
            {
                return BadRequest(
                    new ErrorMessage($"You may only add hands to your own account (ID: {authenticatedUserId})")
                );
            }

            var hand = new Hand(user, body);
            user.Hands.Add(hand);

            try
            {
                int rowsAffected = await _dbContext.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    var handOutputDto = new HandOutputDto(hand);
                    return Ok(handOutputDto);
                }
                else
                {
                    return BadRequest(new ErrorMessage("Failed to add hand"));
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
                        return NotFound(new ErrorMessage($"User ID of \"{userId}\" does not exist."));
                    }

                    query = query.Where(hand => hand.ApplicationUserId == userId);
                }

                List<Hand> hands = await query.ToListAsync();
                List<HandOutputDto> handOutputDtos = hands.Select(hand => new HandOutputDto(hand)).ToList();

                return Ok(handOutputDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var hand = await _dbContext
                    .Hands.Include(h => h.Pots)
                    .Include(h => h.Cards)
                    .Include(h => h.Evaluations)
                    .Include(h => h.Actions)
                    .Include(h => h.PotActions)
                    .FirstOrDefaultAsync(hand => hand.Id == id);

                if (hand == null)
                {
                    return NotFound(new ErrorMessage($"Hand ID of \"{id}\" does not exist."));
                }

                var handOutputDto = new HandOutputDto(hand);

                return Ok(handOutputDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorMessage("Hand ID is required."));
            }

            var hand = await _dbContext.Hands.FirstOrDefaultAsync(h => h.Id == id);
            if (hand == null)
            {
                return NotFound(new ErrorMessage($"Hand ID of \"{id}\" does not exist."));
            }

            var user = await _dbContext
                .ApplicationUsers.Include(u => u.Hands)
                .FirstOrDefaultAsync(u => u.Hands.Any(h => h.Id == id));
            if (user == null)
            {
                return NotFound(new ErrorMessage($"No user associated with hand ID of \"{id}\"."));
            }

            string? authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId != user.Id)
            {
                return BadRequest(
                    new ErrorMessage($"You may only delete hands from your own account (ID: {authenticatedUserId})")
                );
            }

            user.Hands.Remove(hand);

            try
            {
                int rowsAffected = await _dbContext.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new ErrorMessage("Failed to delete hand"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error occurred: {ex.Message}");
            }
        }
    }
}
