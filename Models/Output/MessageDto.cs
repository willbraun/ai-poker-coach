using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ai_poker_coach.Models.Output
{
    public class MessageDto
    {
        public string Id { get; set; } = "";
        public string Object { get; set; } = "";
        public int CreatedAt { get; set; }
        public string AssistantId { get; set; } = "";
        public string ThreadId { get; set; } = "";
        public string RunId { get; set; } = "";
        public string Status { get; set; } = "";
        public object? IncompleteDetails { get; set; }
        public object? IncompleteAt { get; set; }
        public int CompletedAt { get; set; }
        public string Role { get; set; } = "";
        public List<Content> Content { get; set; } = [];
        public List<object> FileIds { get; set; } = [];
        public Metadata Metadata { get; set; } = new();
    }

    public class Content
    {
        public string Type { get; set; } = "";
        public Text Text { get; set; } = new();
    }

    public class Text
    {
        public string Value { get; set; } = "";
        public List<object> Annotations { get; set; } = [];
    }

    public class Metadata
    {
    }
}