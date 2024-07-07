using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ai_poker_coach.Models.DataTransferObjects;

namespace ai_poker_coach.Models.Domain
{
    public class Hand
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public int GameStyle { get; set; }
        public int PlayerCount { get; set; }
        public int Position { get; set; }
        public decimal SmallBlind { get; set; }
        public decimal BigBlind { get; set; }
        public decimal Ante { get; set; }
        public decimal BigBlindAnte { get; set; }
        public decimal MyStack { get; set; }
        public string Notes { get; set; } = "";
        public ICollection<Pot> Pots { get; set; } = [];
        public ICollection<Card> Cards { get; set; } = [];
        public ICollection<Evaluation> Evaluations { get; set; } = [];
        public ICollection<Action> Actions { get; set; } = [];
        public ICollection<PotAction> PotActions { get; set; } = [];
        public string Analysis { get; set; } = "";
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = "";
        public ApplicationUser ApplicationUser { get; set; } = new();

        public Hand() { }

        public Hand(ApplicationUser user, HandInputDto handInputDto)
        {
            if (user.Id != handInputDto.ApplicationUserId)
            {
                throw new Exception(
                    $"User ID {user.Id} does not match Hand DTO user ID {handInputDto.ApplicationUserId}"
                );
            }

            ApplicationUser = user;
            ApplicationUserId = user.Id;
            Name = handInputDto.HandSteps.Name;
            GameStyle = handInputDto.HandSteps.GameStyle;
            PlayerCount = handInputDto.HandSteps.PlayerCount;
            Position = handInputDto.HandSteps.Position;
            SmallBlind = handInputDto.HandSteps.SmallBlind;
            BigBlind = handInputDto.HandSteps.BigBlind;
            Ante = handInputDto.HandSteps.Ante;
            BigBlindAnte = handInputDto.HandSteps.BigBlindAnte;
            MyStack = handInputDto.HandSteps.MyStack;
            Notes = handInputDto.HandSteps.Notes;
            Analysis = handInputDto.Analysis;

            Pots = handInputDto.HandSteps.Pots.Select(potDto => new Pot(this, potDto)).ToList();

            ICollection<Card> cards = [];
            ICollection<Evaluation> evaluations = [];
            ICollection<Action> actions = [];
            ICollection<PotAction> potActions = [];

            foreach (var round in handInputDto.HandSteps.Rounds)
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

            foreach (var villain in handInputDto.HandSteps.Villains)
            {
                cards = [..cards, ..villain.Cards.Select(cardDto => new Card(this, cardDto))];
                evaluations = [..evaluations, new Evaluation(this, villain.Evaluation)];
            }

            Cards = cards;
            Evaluations = evaluations;
            Actions = actions;
            PotActions = potActions;
        }
    }
}
