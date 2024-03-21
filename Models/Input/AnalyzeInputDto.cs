using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Input
{
    public class AnalyzeInputDto
    {
        public string Name { get; set; } = "";
        public int GameStyle { get; set; }
        public int PlayerCount { get; set; }
        public int Position { get; set; }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }
        public int Ante { get; set; }
        public int BigBlindAnte { get; set; }
        public int MyStack { get; set; }
        public string PlayerNotes { get; set; } = "";
        public string Winners { get; set; } = "";
        public List<RoundInputDto> Rounds { get; set; } = [];
        public List<VillainInputDto> Villains { get; set; } = [];
    }

    public class RoundInputDto
    {
        public List<CardInputDto> Cards { get; set; } = [];
        public EvaluationInputDto Evaluation { get; set; } = new();
        public List<ActionInputDto> Actions { get; set; } = [];
    }

    public class VillainInputDto
    {
        public List<CardInputDto> Cards { get; set; } = [];
        public EvaluationInputDto Evaluation { get; set; } = new();
    }
}