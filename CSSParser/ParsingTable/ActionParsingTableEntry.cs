using CSSParser.Instances;
using System;
using System.Collections.Generic;

namespace CSSParser
{
    public class ActionParsingTableEntry : ParsingTableEntry
    {
        public string ActionDescription { get; set; }
        public Func<ActionParsingTableEntry, bool> Action { get; set; }
        public TerminalExpressionDefinition ExpressionDefinition { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        public override string ToString()
        {
            return $"ACTION {ItemSet.Id}, {ExpressionDefinition.Key}, {ActionDescription}";
        }

        internal override string ShortDescription()
        {
            return $"{ActionDescription}";
        }
    }
}
