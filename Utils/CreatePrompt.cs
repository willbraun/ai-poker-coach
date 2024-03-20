using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;
using ai_poker_coach.Models.Input;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace ai_poker_coach.Utils
{
    public class PromptUtils
    {
        public static string CreatePrompt(AnalyzeInputDto requestBody)
        {
            List<string> gameStyles = ["Cash Game", "Tournament"];
            Dictionary<string, string> values = new() {
                {"2", "Two"},
                {"3", "Three"},
                {"4", "Four"},
                {"5", "Five"},
                {"6", "Six"},
                {"7", "Seven"},
                {"8", "Eight"},
                {"9", "Nine"},
                {"T", "Ten"},
                {"J", "Jack"},
                {"Q", "Queen"},
                {"K", "King"},
                {"A", "Ace"},
            };
            Dictionary<string, string> suits = new() {
                {"C", "Clubs"},
                {"D", "Diamonds"},
                {"H", "Hearts"},
                {"S", "Spades"},
            };

            string initial = $@"
            Game style: {gameStyles[requestBody.GameStyle]}
            Players: {requestBody.PlayerCount}
            Position relative to Small Blind (1) and Button({requestBody.PlayerCount}): {requestBody.Position}
            Small Blind: {requestBody.SmallBlind}
            Big Blind: {requestBody.BigBlind}
            Ante: {requestBody.Ante}
            Big Blind Ante: {requestBody.BigBlindAnte}
            My Stack: {requestBody.MyStack}
            Player Notes: {requestBody.PlayerNotes}
            Winners: {requestBody.Winners}
            
            Hand Action:";

            string message = TrimLeadingSpaces(initial);

            CardInputDto hero1 = requestBody.Cards.Hero[0];
            CardInputDto hero2 = requestBody.Cards.Hero[1];
            message += $"\nDealer deals me: {values[hero1.Value]} of {suits[hero1.Suit]} and {values[hero2.Value]} of {suits[hero2.Suit]}\n";
            message += GetActionMessages(requestBody.Actions.Preflop);

            if (requestBody.Cards.Flop.Count == 0) return message;
            CardInputDto flop1 = requestBody.Cards.Flop[0];
            CardInputDto flop2 = requestBody.Cards.Flop[1];
            CardInputDto flop3 = requestBody.Cards.Flop[2];
            message += $"Dealer deals the flop: {values[flop1.Value]} of {suits[flop1.Suit]}, {values[flop2.Value]} of {suits[flop2.Suit]}, and {values[flop3.Value]} of {suits[flop3.Suit]}.\n";
            message += GetActionMessages(requestBody.Actions.Flop);

            if (requestBody.Cards.Turn.Count == 0) return message;
            CardInputDto turn = requestBody.Cards.Turn[0];
            message += $"Dealer deals the turn: {values[turn.Value]} of {suits[turn.Suit]}.\n";
            message += GetActionMessages(requestBody.Actions.Turn);

            if (requestBody.Cards.River.Count == 0) return message;
            CardInputDto river = requestBody.Cards.River[0];
            message += $"Dealer deals the river: {values[river.Value]} of {suits[river.Suit]}.\n";
            message += GetActionMessages(requestBody.Actions.River);

            if (requestBody.Cards.Villain.Count == 0) return message;
            for (int j = 0; j < requestBody.Cards.Villain.Count; j += 2)
            {
                CardInputDto villian1 = requestBody.Cards.Villain[j];
                CardInputDto villian2 = requestBody.Cards.Villain[j + 1];
                message += $"Player {villian1.Player} shows down: {values[villian1.Value]} of {suits[villian1.Suit]} and {values[villian2.Value]} of {suits[villian2.Suit]}\n";
            }

            message += $"Hand winners: {string.Join(", ", requestBody.Winners.Split(",").Select(winner => $"Player {winner}"))}";

            return message;
        }

        static string TrimLeadingSpaces(string input)
        {
            string[] lines = input.Trim().Split('\n');
            return string.Join(Environment.NewLine, lines.Select(line => line.TrimStart()));
        }

        static string GetActionMessages(List<ActionInputDto> actions)
        {
            List<string> decisions = ["folds", "checks", "calls", "bets"];

            string addition = "";
            foreach (var action in actions)
            {
                string betMessage = action.Decision > 1 ? $" {action.Bet}." : ".";
                addition += $"Player {action.Player} {decisions[action.Decision]}{betMessage}\n";
            }

            return addition;
        }
    }
}