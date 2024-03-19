using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ai_poker_coach.Models.Input;

namespace ai_poker_coach.Utils
{
    public class Utils
    {
        public static string CreatePrompt(AnalyzeInputDto requestBody)
        {
            List<string> gameStyles = ["Cash Game", "Tournament"];

            string initial = $@"
            Game style: {gameStyles[requestBody.GameStyle]}
            Players: {requestBody.PlayerCount}
            Position relative to small blind (1): {requestBody.Position}
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
            foreach (IHandStepInputDto step in sortedSteps)
            {
                if (step is ActionInputDto)
                {
                    message += "\naction";
                }
                else if (step is CardInputDto)
                {
                    message += "\ncard";
                }
                else
                {
                    message += "\ninvalid step type";
                }
            }

            return message;
        }

        static string TrimLeadingSpaces(string input)
        {
            string[] lines = input.Trim().Split('\n');
            return string.Join(Environment.NewLine, lines.Select(line => line.TrimStart()));
        }
    }
}