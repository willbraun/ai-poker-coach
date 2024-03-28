using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Domain;
using ai_poker_coach.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace ai_poker_coach.Utils
{
    public class PromptUtils
    {
        private static List<decimal> pots = [0];

        public static string CreatePrompt(HandStepsDto body)
        {
            List<string> gameStyles = ["Cash Game", "Tournament"];
            List<string> deals = ["me", "the flop", "the turn", "the river"];

            pots[0] = (decimal)(body.SmallBlind + body.BigBlind + (body.Ante * body.PlayerCount) + body.BigBlindAnte)!;

            string initial = $@"
            Game style: {gameStyles[body.GameStyle ?? 0]}
            Players: {body.PlayerCount}
            Position relative to Small Blind (1) and Button({body.PlayerCount}): {body.Position}
            Small Blind: {body.SmallBlind}
            Big Blind: {body.BigBlind}
            Ante: {body.Ante}
            Big Blind Ante: {body.BigBlindAnte}
            My Stack: {body.MyStack}
            Player Notes: {body.PlayerNotes}
            Winners: {body.Winners}

            Starting pot size after blinds and antes: {pots[0]}
            
            Hand Action:";

            string message = TrimLeadingSpaces(initial) + "\n";

            for (int i = 0; i < body.Rounds!.Count; i++)
            {
                if (body.Rounds[i].Cards.Count == 0) break;
                message += $"Dealer deals {deals[i]}: {ListCards(body.Rounds[i].Cards)}\n";
                message += $"My CURRENT HAND is now: {body.Rounds[i].Evaluation.Value}\n";
                message += GetActionMessages(body.Rounds[i].Actions, body.Position ?? 0);
            }

            foreach (var villain in body.Villains)
            {
                message += $"Player {villain.Cards[0].Player} shows down: {ListCards([villain.Cards[0], villain.Cards[1]])}\n";
                message += $"Player {villain.Cards[0].Player}'s CURRENT HAND is: {villain.Evaluation.Value}\n";
            }

            message += "Hand winners: " + string.Join(", ", body.Winners!.Split(",").Select(winner => "Player " + winner + (winner == body.Position.ToString() ? " (me)" : "")));

            return message;
        }

        static string TrimLeadingSpaces(string input)
        {
            string[] lines = input.Trim().Split('\n');
            return string.Join(Environment.NewLine, lines.Select(line => line.TrimStart()));
        }

        public static string ListCards(List<CardDto> cards)
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

            return string.Join(", ", cards.Select(card => $"{values[card.Value!]} of {suits[card.Suit!]}"));
        }

        static string GetActionMessages(List<ActionDto> actions, int myPosition)
        {
            List<string> decisions = ["folds", "checks", "calls", "bets", "bets all-in for", "calls all-in for"];

            string addition = "";
            foreach (var action in actions)
            {
                foreach (var potAction in action.PotActions ?? [])
                {
                    if (potAction.PotIndex == null || potAction.Bet == null){
                        break;
                    }

                    if (potAction.PotIndex > pots.Count - 1)
                    {
                        pots.Add((decimal)potAction.Bet);
                    }
                    else
                    {
                        pots[(int)potAction.PotIndex] += (decimal)potAction.Bet;
                    }

                }
                string betSize = action.Decision > 1 ? $" {action.Bet}" : "";
                addition += $"Player {action.Player}{(action.Player == myPosition ? " (me)" : "")} {decisions[action.Decision ?? 0]}{betSize}.";

                for (int i = 0; i < pots.Count; i++)
                {
                    string potName = "Main pot";
                    if (i > 0)
                    {
                        potName = $"Side pot {i}";
                    }

                    addition += $" {potName} is now {pots[i]}. ";
                }

                addition += "\n";
            }

            return addition;
        }
    }
}