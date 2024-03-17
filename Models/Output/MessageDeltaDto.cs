using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Output
{
    public class MessageDeltaDto
    {
        public Data Data { get; set; } = new();
        public string Event = "thread.message.delta";
    }

    public class Data
    {
        public string Id { get; set; } = "";
        public string Object { get; set; } = "";
        public Delta Delta { get; set; } = new();
    }

    public class Delta
    {
        public List<Content> Content { get; set; } = [];
    }

    public class Content
    {
        public int Index { get; set; }
        public string Type { get; set; } = "";
        public Text Text { get; set; } = new();
    }

    public class Text
    {
        public string Value { get; set; } = "";
        public List<object> Annotations { get; set; } = [];
    }


}