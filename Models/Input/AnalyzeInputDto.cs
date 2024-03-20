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
        public ActionGroupInputDto Actions { get; set; } = new();
        public CardGroupInputDto Cards { get; set; } = new();
    }

    public class ActionGroupInputDto
    {
        public List<ActionInputDto> Preflop { get; set; } = [];
        public List<ActionInputDto> Flop { get; set; } = [];
        public List<ActionInputDto> Turn { get; set; } = [];
        public List<ActionInputDto> River { get; set; } = [];
    }

    public class CardGroupInputDto
    {
        public List<CardInputDto> Hero { get; set; } = [];
        public List<CardInputDto> Flop { get; set; } = [];
        public List<CardInputDto> Turn { get; set; } = [];
        public List<CardInputDto> River { get; set; } = [];
        public List<CardInputDto> Villain { get; set; } = [];
    }

}