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
            List<string> deals = ["me", "the flop", "the turn", "the river"];

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

            string message = TrimLeadingSpaces(initial) + "\n";

            for (int i = 0; i < requestBody.Rounds.Count; i++)
            {
                if (requestBody.Rounds[i].Cards.Count == 0) break;
                message += $"Dealer deals {deals[i]}: {ListCards(requestBody.Rounds[i].Cards)}\n";
                message += $"My CURRENT HAND is now: {requestBody.Rounds[i].Evaluation.Value}\n";
                message += GetActionMessages(requestBody.Rounds[i].Actions, requestBody.Position);
            }

            foreach (var villain in requestBody.Villains)
            {
                message += $"Player {villain.Cards[0].Player} shows down: {ListCards([villain.Cards[0], villain.Cards[1]])}\n";
                message += $"Player {villain.Cards[0].Player}'s CURRENT HAND is: {villain.Evaluation.Value}\n";
            }

            message += "Hand winners: " + string.Join(", ", requestBody.Winners.Split(",").Select(winner => "Player " + winner + (winner == requestBody.Position.ToString() ? " (me)" : "" )));

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

        static string GetActionMessages(List<ActionInputDto> actions, int myPosition)
        {
            List<string> decisions = ["folds", "checks", "calls", "bets", "bets all-in for", "calls all-in for"];

            string addition = "";
            foreach (var action in actions)
            {
                string betSize = action.Decision > 1 ? $" {action.Bet}." : ".";
                addition += $"Player {action.Player}{(action.Player == myPosition ? " (me)" : "")} {decisions[action.Decision]}{betSize}\n";
            }

            return addition;
        }
    }
}