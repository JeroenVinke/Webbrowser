using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class TagRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            //grammar.Add(new Production(ParserConstants.ElementCloseRule,
            //    new List<SubProduction>
            //    {
            //        ElementCloseRule()
            //    }
            //));
            grammar.Add(new Production(ParserConstants.ElementsRule,
                new List<SubProduction>
                {
                    ElementRule(),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode("", new List<ElementASTNode>()) { });
                            })
                        }
                    )
                }
            ));
            grammar.Add(new Production(ParserConstants.ElementRule,
                new List<SubProduction>
                {
                    ElementRule()
                }
            ));
        }


        private static SubProduction ElementRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.OpenTag },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementsRule },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode("", new List<ElementASTNode>()) { });
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.CloseTag },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string elementName = node.GetAttributeForKey<WordToken>("OpenTag", ParserConstants.Token).Lexeme;

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(elementName.Replace("<","").Replace(">","")) { });
                    })
                }
            );
        }
    }
}