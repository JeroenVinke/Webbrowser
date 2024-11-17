using Compiler.Common;
using HTMLParser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace HTMLParser.Rules
{
    public class ElementsRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.ElementsRule,
                new List<SubProduction>
                {
                    new SubProduction(new List<ExpressionDefinition>()
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementRule },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementsRule },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            var children = new List<ElementASTNode>();

                            children.Add(node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementRule, ParserConstants.SyntaxTreeNode));
                            children.AddRange(node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementsRule, ParserConstants.SyntaxTreeNode).Children);

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
            grammar.Add(new Production(ParserConstants.ElementRule,
                new List<SubProduction>
                {
                    ElementRule(),
                    ElementWithIdentifierRule(),
                    SelfClosingElementRule()
                }
            ));
            grammar.Add(new Production(ParserConstants.TopElementRule,
                new List<SubProduction>
                {
                    ElementRule()
                }
            ));
        }


        private static SubProduction ElementWithIdentifierRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.OpenTag },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.CloseTag },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string rawHtmlElement = node.GetAttributeForKey<WordToken>("OpenTag", ParserConstants.Token).Lexeme;

                        var htmlElement = new HtmlElement(rawHtmlElement);

                        string innerText = node.GetAttributeForKey<WordToken>("Identifier", ParserConstants.Token).Lexeme;

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(htmlElement.Name, htmlElement.Attributes, new List<ElementASTNode>(), innerText) { });
                    })
                }
            );
        }


        private static SubProduction ElementRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.OpenTag },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ElementsRule },
                    new TerminalExpressionDefinition { TokenType = TokenType.CloseTag },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string rawHtmlElement = node.GetAttributeForKey<WordToken>("OpenTag", ParserConstants.Token).Lexeme;

                        var htmlElement = new HtmlElement(rawHtmlElement);

                        ElementASTNode elementsNode = node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementsRule, ParserConstants.SyntaxTreeNode);
                        
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(htmlElement.Name, htmlElement.Attributes, elementsNode.Children) { });
                    })
                }
            );
        }


        private static SubProduction SelfClosingElementRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.SelfClosingTag },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string rawHtmlElement = node.GetAttributeForKey<WordToken>("SelfClosingTag", ParserConstants.Token).Lexeme;

                        var htmlElement = new HtmlElement(rawHtmlElement);

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(htmlElement.Name, htmlElement.Attributes, new List<ElementASTNode>()) { });
                    })
                }
            );
        }
    }
}