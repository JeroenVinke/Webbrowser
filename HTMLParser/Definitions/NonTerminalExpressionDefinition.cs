﻿using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HTMLParser
{
    public class NonTerminalExpressionDefinition : ExpressionDefinition
    {
        private string identifier;
        public string Identifier
        {
            get => identifier;
            set
            {
                identifier = value;
                if (string.IsNullOrEmpty(Key))
                {
                    Key = value;
                }
            }
        }

        private ExpressionSet _first;

        public ExpressionSet Follow()
        {
            ExpressionSet result = new ExpressionSet();
            result.AddRangeUnique(GetFollow());
            return result;
        }

        private ExpressionSet GetFollow(List<NonTerminalExpressionDefinition> visited = null)
        {
            visited = visited ?? new List<NonTerminalExpressionDefinition>();

            if (visited.Any(x => x.IsEqualTo(this)))
            {
                return new ExpressionSet();
            }

            visited.Add(this);

            ExpressionSet result = new ExpressionSet();

            foreach (Production production in Grammar.Instance)
            {
                foreach (SubProduction subProduction in production)
                {
                    bool found = false;

                    ExpressionDefinition last = null;

                    foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticActionDefinition)))
                    {
                        last = expression;

                        if (expression is NonTerminalExpressionDefinition ne && ne.Identifier == Identifier)
                        {
                            found = true;
                        }
                        else 
                        {
                            if (found)
                            {
                                if (expression.First().Any(x => x.TokenType == TokenType.EmptyString)
                                    && ((NonTerminalExpressionDefinition)expression).Identifier != Identifier)
                                {
                                    result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow(visited));
                                }

                                result.AddRangeUnique(new ExpressionSet(expression.First().Where(x => x.TokenType != TokenType.EmptyString).ToList()));
                                found = false;
                            }
                        }
                    }

                    if (found && production.Identifier == Identifier && last is NonTerminalExpressionDefinition nte)
                    {
                        result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = nte.Identifier }.First());
                    }

                    // when last
                    if (found)
                    {
                        result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow(visited));
                    }
                }
            }

            // when last
            if (Identifier == ParserConstants.Initial)
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EndMarker });
            }

            return result;
        }

        public override ExpressionSet First()
        {
            if (_first != null)
            {
                return _first;
            }

            ExpressionSet result = new ExpressionSet();

            //if (Grammar.Instance.Any(x => x.Identifier == Identifier
            //    && x.Any(y => y.Count == 1
            //    && y.First() is TerminalExpressionDefinition te
            //    && te.TokenType == TokenType.EmptyString)))
            //{
            //    result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EmptyString });
            //}

            foreach (SubProduction subProduction in Grammar.Instance.First(x => x.Identifier == Identifier))
            {
                bool canContinue = true;

                foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticActionDefinition)))
                {
                    if (expression.IsEqualTo(this))
                    {
                        break;
                    }

                    if (canContinue)
                    {
                        ExpressionSet first = expression.First();

                        result.AddRangeUnique(first);

                        canContinue = first.Any(x => x.TokenType == TokenType.EmptyString);
                    }
                }
            }

            _first = result;

            return result;
        }

        public override string ToString()
        {
            return Identifier;
        }

        public override bool IsEqualTo(ExpressionDefinition definition)
        {
            if (definition is NonTerminalExpressionDefinition nte)
            {
                return Identifier == nte.Identifier;
            }

            return false;
        }
    }
}