using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text;
using static ai_poker_coach.Utils.PromptUtils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ai_poker_coach.Models.Domain;
using Action = ai_poker_coach.Models.Domain.Action;
using DotNet8Authentication.Data;

namespace ai_poker_coach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        private readonly IHttpClientFactory
        _httpClientFactory;

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

                response.EnsureSuccessStatusCode();

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
                return StatusCode(500, $"An error occurred while analyzing data: {ex.Message}");
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

            var user = _dbContext.ApplicationUsers.First(u => u.Id == body.ApplicationUserId);
            if (user == null)
            {
                return NotFound($"User ID of {body.ApplicationUserId} does not exist.");
            }

            Hand hand = new()
            {
                ApplicationUser = user,
                ApplicationUserId = body.ApplicationUserId!,
                Name = body.HandSteps!.Name,
                GameStyle = body.HandSteps.GameStyle ?? 0,
                PlayerCount = body.HandSteps.PlayerCount ?? 0,
                Position = body.HandSteps.Position ?? 0,
                SmallBlind = body.HandSteps.SmallBlind ?? 0,
                BigBlind = body.HandSteps.BigBlind ?? 0,
                Ante = body.HandSteps.Ante ?? 0,
                BigBlindAnte = body.HandSteps.BigBlindAnte ?? 0,
                MyStack = body.HandSteps.MyStack ?? 0,
                PlayerNotes = body.HandSteps.PlayerNotes!,
                Analysis = body.Analysis!,
            };

            ICollection<Pot> pots = body.HandSteps!.Pots!.Select(potDto => new Pot
            {
                Hand = hand,
                HandId = hand.HandId,
                Winner = potDto.Winner!
            }).ToList();

            ICollection < Card> cards = [];
            ICollection<Evaluation> evaluations = [];
            ICollection<Action> actions = [];
            ICollection<PotAction> potActions = [];

            foreach (var round in body.HandSteps!.Rounds!)
            {
                cards = [..cards, ..round.Cards.Select(cardDto => new Card {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = cardDto.Step ?? 0,
                    Player = cardDto.Player ?? 0,
                    Value = cardDto.Value!,
                    Suit = cardDto.Suit!,
                })];

                evaluations = [..evaluations, new Evaluation {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = round.Evaluation.Step ?? 0,
                    Player = round.Evaluation.Player ?? 0,
                    Value = round.Evaluation.Value!
                }];

                actions = [..actions, ..round.Actions.Select(actionDto => new Action {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = actionDto.Step ?? 0,
                    Player = actionDto.Player ?? 0,
                    Decision = actionDto.Decision ?? 0,
                    Bet = actionDto.Bet ?? 0,
                })];

                potActions = [..potActions, ..round.PotActions.Select(potActionDto => new PotAction {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = potActionDto.Step ?? 0,
                    Player = potActionDto.Player ?? 0,
                    Pot = pots.ElementAt((int)potActionDto.PotIndex!),
                    PotId = pots.ElementAt((int)potActionDto.PotIndex!).PotId,
                    Bet = potActionDto.Bet ?? 0,
                })];
            }

            foreach (var villain in body.HandSteps.Villains)
            {
                cards = [..cards, ..villain.Cards.Select(cardDto => new Card {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = cardDto.Step ?? 0,
                    Player = cardDto.Player ?? 0,
                    Value = cardDto.Value!,
                    Suit = cardDto.Suit!,
                })];

                evaluations = [..evaluations, new Evaluation {
                    Hand = hand,
                    HandId = hand.HandId,
                    Step = villain.Evaluation.Step ?? 0,
                    Player = villain.Evaluation.Player ?? 0,
                    Value = villain.Evaluation.Value!
                }];
            }

            hand.Pots = pots;
            hand.Cards = cards;
            hand.Evaluations = evaluations;
            hand.Actions = actions;
            hand.PotActions = potActions;

            user.Hands.Add(hand);

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
                return StatusCode(500, $"Database error occurred: {ex.Message}");
            }
        }
    }
}