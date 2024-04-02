using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Hand
    {
        [Key]
        public int HandId { get; set; }
        public string Name { get; set; } = "";
        public int GameStyle { get; set; }
        public int PlayerCount { get; set; }
        public int Position { get; set; }
        public decimal SmallBlind { get; set; }
        public decimal BigBlind { get; set; }
        public decimal Ante { get; set; }
        public decimal BigBlindAnte { get; set; }
        public decimal MyStack { get; set; }
        public string PlayerNotes { get; set; } = "";
        public ICollection<Pot> Pots { get; set; } = [];
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<Evaluation> Evaluations { get; set; } = [];
        public ICollection<Action> Actions { get; set; } = [];
        public ICollection<PotAction> PotActions { get; set; } = [];
        public string Analysis { get; set; } = "";
        public DateTime CreatedTime { get; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = "";
        public ApplicationUser ApplicationUser { get; set; } = new();

        public Hand()
        {
            CreatedTime = DateTime.UtcNow;
        }

        public Hand(ApplicationUser user, HandDto handDto)
        {
            if (user.Id != handDto.ApplicationUserId)
            {
                throw new Exception($"User ID {user.Id} does not match Hand DTO user ID {handDto.ApplicationUserId}");
            }

            ApplicationUser = user;
            ApplicationUserId = user.Id!;
            Name = handDto.HandSteps!.Name;
            GameStyle = handDto.HandSteps.GameStyle ?? 0;
            PlayerCount = handDto.HandSteps.PlayerCount ?? 0;
            Position = handDto.HandSteps.Position ?? 0;
            SmallBlind = handDto.HandSteps.SmallBlind ?? 0;
            BigBlind = handDto.HandSteps.BigBlind ?? 0;
            Ante = handDto.HandSteps.Ante ?? 0;
            BigBlindAnte = handDto.HandSteps.BigBlindAnte ?? 0;
            MyStack = handDto.HandSteps.MyStack ?? 0;
            PlayerNotes = handDto.HandSteps.PlayerNotes!;
            Analysis = handDto.Analysis!;

            Pots = handDto.HandSteps!.Pots!.Select(potDto => new Pot(this, potDto)).ToList();

            ICollection<Card> cards = [];
            ICollection<Evaluation> evaluations = [];
            ICollection<Action> actions = [];
            ICollection<PotAction> potActions = [];

            foreach (var round in handDto.HandSteps!.Rounds!)
            {
                cards = [..cards, ..round.Cards.Select(cardDto => new Card(this, cardDto))];
                evaluations = [..evaluations, new Evaluation(this, round.Evaluation)];
                actions = [..actions, ..round.Actions.Select(actionDto => new Action(this, actionDto))];
                potActions =
                [
                    ..potActions,
                    ..round.PotActions.Select(potActionDto => new PotAction(this, potActionDto))
                ];
            }

            foreach (var villain in handDto.HandSteps.Villains)
            {
                cards = [..cards, ..villain.Cards.Select(cardDto => new Card(this, cardDto))];
                evaluations = [..evaluations, new Evaluation(this, villain.Evaluation)];
            }

            Cards = cards;
            Evaluations = evaluations;
            Actions = actions;
            PotActions = potActions;
            CreatedTime = DateTime.UtcNow;
        }
    }
}
