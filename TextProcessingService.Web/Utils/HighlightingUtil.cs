using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextProcessingService.Domain.Models;

namespace TextProcessingService.Web.Utils
{
    /// <summary>
    /// Утилита для выделения объектов цветом
    /// </summary>
    public class HighlightingUtil
    {
        public static string SetHighlighting(string text, List<Person> persons)
        {
            var highlightingPositions = GetHighlightingPositions(persons)
                .OrderBy(hp => hp.Position.LastChar)
                .ThenBy(hp=>hp.Position.FirstChar);
            int lastChar = 0;
            int difference = 0;
            foreach (var highlightingPosition in highlightingPositions)
            {
                string openTag = $"<span style=\"color: {highlightingPosition.Color};\">";
                text = text
                    .Insert(Math.Max(lastChar, highlightingPosition.Position.LastChar + 1) + difference, "</span>")
                    .Insert(Math.Max(lastChar, highlightingPosition.Position.FirstChar) + difference, $"<span style=\"color: {highlightingPosition.Color};\">");
                lastChar = Math.Max(lastChar, highlightingPosition.Position.LastChar + 1);
                difference += openTag.Length + 7;
            }
            return text;
        }

        private static List<HighlightingPosition> GetHighlightingPositions(List<Person> persons)
        {
            var highlightingPositions = new List<HighlightingPosition>();
            foreach (var person in persons)
            {
                person.Positions.ForEach(p => highlightingPositions.Add(new HighlightingPosition(p, "red")));
                foreach (var personProperty in person.Properties)
                {
                    personProperty.Positions.ForEach(p => highlightingPositions.Add(new HighlightingPosition(p, "green")));
                }
            }
            return highlightingPositions;
        }
    }

    public class HighlightingPosition
    {
        public HighlightingPosition(Position position, string color)
        {
            Position = position;
            Color = color;
        }

        public Position Position { get; set; }
        public string Color { get; set; }
    }
}