using Compiler.Common;
using CSSParser.Rules;
using System.Collections.Generic;
using System.Linq;

namespace CSSParser
{
    public class Grammar : List<Production>
    {
        private static Grammar _instance;
        public static Grammar Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Grammar();

                    _instance.Add(new Production(ParserConstants.Initial, new List<SubProduction> {
                        new SubProduction(new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.RuleSetsRule }
                        })
                    }));

                    CSSRule.Initialize(ref _instance);
                }

                return _instance;
            }
        }

        public List<ExpressionDefinition> Symbols()
        {
            List<ExpressionDefinition> result = new List<ExpressionDefinition>();

            foreach (Production production in this)
            {
                foreach(SubProduction subProduction in production)
                {
                    foreach(ExpressionDefinition expressionDefinition in subProduction.Where(x => !(x is SemanticActionDefinition)))
                    {
                        if (!result.Any(x => x.IsEqualTo(expressionDefinition)))
                        {
                            result.Add(expressionDefinition);
                        }
                    }
                }
            }

            result.Add(new TerminalExpressionDefinition() { TokenType = TokenType.EndMarker });

            return result;
        }
    }
}
