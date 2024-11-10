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
                                //node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ClassesASTNode());
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
            grammar.Add(new Production(ParserConstants.TopElementRule,
                new List<SubProduction>
                {
                    TopElementRule()
                }
            ));
        }

        private static SubProduction TopElementRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementRule }
                }
            );
        }

        private static SubProduction ElementRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.LessThan },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.GreaterThan },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementsRule },
                    new TerminalExpressionDefinition { TokenType = TokenType.LessThan },
                    new TerminalExpressionDefinition { TokenType = TokenType.ForwardSlash },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier, Key = "ElementName" },
                    new TerminalExpressionDefinition { TokenType = TokenType.GreaterThan },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string elementName = node.GetAttributeForKey<WordToken>("ElementName", ParserConstants.Token).Lexeme;

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(elementName) { });
                    })
                }
            );
        }

        private static SubProduction ElementCloseRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.LessThan },
                    new TerminalExpressionDefinition { TokenType = TokenType.ForwardSlash },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string elementName = node.GetAttributeForKey<WordToken>("Identifier", ParserConstants.Token).Lexeme;

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(elementName) { });
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.GreaterThan },
                }
            );
        }

    }
}