﻿using System;

namespace CSSParser
{
    public class SemanticActionDefinition : ExpressionDefinition
    {
        public Action<ParsingNode> Action { get; set; }

        public SemanticActionDefinition(Action<ParsingNode> action)
        {
            Action = action;
        }

        public override ExpressionSet First()
        {
            return null;
        }

        public override string ToString()
        {
            return "Action";
        }

        public override bool IsEqualTo(ExpressionDefinition definition)
        {
            return true;
        }
    }
}