using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Domain.Models;
using ai_poker_coach.Models.Input;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace ai_poker_coach.Utils
{
    public class PromptUtils
    {
        public static string CreatePrompt(AnalyzeInputDto requestBody)
        {
            List<string> gameStyles = ["Cash Game", "Tournament"];
            List<string> actions = ["folds", "checks", "calls", "bets"];
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

            List<IHandStepInputDto> steps = [.. requestBody.Actions, .. requestBody.Cards];
            List<IHandStepInputDto> sortedSteps = [.. steps.OrderBy(step => step.Step)];

            int i = 0;

            CardInputDto hole1 = (CardInputDto)sortedSteps[i];
            CardInputDto hole2 = (CardInputDto)sortedSteps[i + 1];
            message += $"\nDealer deals me: {values[hole1.Value]} of {suits[hole1.Suit]} and {values[hole2.Value]} of {suits[hole2.Suit]}\n";
            i += 2;

            while (sortedSteps[i] is ActionInputDto current)
            {
                string betMessage = current.Decision > 1 ? $" {current.Bet}." : ".";
                message += $"Player {current.Player} {actions[current.Decision]}{betMessage}\n";
                i++;
            }

            if (i >= steps.Count - 1) return message;

            CardInputDto flop1 = (CardInputDto)sortedSteps[i];
            CardInputDto flop2 = (CardInputDto)sortedSteps[i + 1];
            CardInputDto flop3 = (CardInputDto)sortedSteps[i + 2];
            message += $"Dealer deals the flop: {values[flop1.Value]} of {suits[flop1.Suit]}, {values[flop2.Value]} of {suits[flop2.Suit]}, and {values[flop3.Value]} of {suits[flop3.Suit]}.\n";
            i += 3;

            while (sortedSteps[i] is ActionInputDto current)
            {
                string betMessage = current.Decision > 1 ? $" {current.Bet}." : ".";
                message += $"Player {current.Player} {actions[current.Decision]}{betMessage}\n";
                i++;
            }

            if (i >= steps.Count - 1) return message;

            CardInputDto turn = (CardInputDto)sortedSteps[i];
            message += $"Dealer deals the turn: {values[turn.Value]} of {suits[turn.Suit]}.\n";
            i++;

            while (sortedSteps[i] is ActionInputDto current)
            {
                string betMessage = current.Decision > 1 ? $" {current.Bet}." : ".";
                message += $"Player {current.Player} {actions[current.Decision]}{betMessage}\n";
                i++;
            }

            if (i >= steps.Count - 1) return message;

            CardInputDto river = (CardInputDto)sortedSteps[i];
            message += $"Dealer deals the river: {values[river.Value]} of {suits[river.Suit]}.\n";
            i++;

            while (sortedSteps[i] is ActionInputDto current)
            {
                string betMessage = current.Decision > 1 ? $" {current.Bet}." : ".";
                message += $"Player {current.Player} {actions[current.Decision]}{betMessage}\n";
                i++;
            }

            if (i >= steps.Count - 1) return message;

            var villainCards = sortedSteps.Skip(i).OrderBy(card => card.Player).ToList();
            for (int j = 0; j < villainCards.Count; j += 2)
            {
                CardInputDto villian1 = (CardInputDto)villainCards[j];
                CardInputDto villian2 = (CardInputDto)villainCards[j + 1];
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
    }
}