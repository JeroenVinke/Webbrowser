using Compiler.Common;
using CSSParser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace CSSParser.Rules
{
    public class CSSRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.RuleSetsRule,
                new List<SubProduction>
                {
                    new SubProduction(new List<ExpressionDefinition>()
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.RuleSetRule },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.RuleSetsRule },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            var children = new List<ElementASTNode>();

                            //children.Add(node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementRule, ParserConstants.SyntaxTreeNode));
                            //children.AddRange(node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementsRule, ParserConstants.SyntaxTreeNode).Children);

                            ElementASTNode astNode = new ElementASTNode("", children);
                            
                            node.Attributes.Add(ParserConstants.SyntaxTreeNode, astNode);
                        })
                    }),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode("",  new List<ElementASTNode>()) { });
                            })
                        }
                    )
                }
            ));
            grammar.Add(new Production(ParserConstants.RuleSetRule,
                new List<SubProduction>
                {
                    RuleSetRule()
                }
            ));
        }


        private static SubProduction RuleSetRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketOpen },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.Colon },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketClosed },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        //string rawHtmlElement = node.GetAttributeForKey<WordToken>("OpenTag", ParserConstants.Token).Lexeme;

                        //var htmlElement = new HtmlElement(rawHtmlElement);

                        //ElementASTNode elementsNode = node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementsRule, ParserConstants.SyntaxTreeNode);
                        
                        //node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(htmlElement.Name, htmlElement.Attributes, elementsNode.Children) { });
                    })
                }
            );
        }
    }
}