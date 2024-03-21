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

            message += $"\nDealer deals me: {ListCards(requestBody.Cards.Preflop)}\n";
            message += $"My CURRENT HAND is now: {requestBody.Evaluations.Preflop}\n";
            message += GetActionMessages(requestBody.Actions.Preflop);

            if (requestBody.Cards.Flop.Count == 0) return message;
            message += $"Dealer deals the flop: {ListCards(requestBody.Cards.Flop)}.\n";
            message += $"My CURRENT HAND is now: {requestBody.Evaluations.Flop}\n";
            message += GetActionMessages(requestBody.Actions.Flop);

            if (requestBody.Cards.Turn.Count == 0) return message;
            message += $"Dealer deals the turn: {ListCards(requestBody.Cards.Turn)}.\n";
            message += $"My CURRENT HAND is now: {requestBody.Evaluations.Turn}\n";
            message += GetActionMessages(requestBody.Actions.Turn);

            if (requestBody.Cards.River.Count == 0) return message;
            message += $"Dealer deals the river: {ListCards(requestBody.Cards.River)}.\n";
            message += $"My CURRENT HAND is now: {requestBody.Evaluations.River}\n";
            message += GetActionMessages(requestBody.Actions.River);

            if (requestBody.Cards.Villains.Count == 0) return message;
            foreach (var card in requestBody.Cards.Villains)
            {
                CardInputDto villain1 = card[0];
                CardInputDto villain2 = card[1];
                message += $"Player {villain1.Player} shows down: {ListCards([villain1, villain2])}\n";
                message += $"Player {villain1.Player}'s CURRENT HAND is: {requestBody.Evaluations.Villains.Find(vilEval => vilEval.Player == villain1.Player)?.Evaluation ?? ""}\n";
            }

            message += $"Hand winners: {string.Join(", ", requestBody.Winners.Split(",").Select(winner => $"Player {winner}"))}";

            return message;
        }

        static string TrimLeadingSpaces(string input)
        {
            string[] lines = input.Trim().Split('\n');
            return string.Join(Environment.NewLine, lines.Select(line => line.TrimStart()));
        }

        static string ListCards(List<CardInputDto> cards)
        {
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

            return string.Join(", ", cards.Select(card => $"{values[card.Value]} of {suits[card.Suit]}"));
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