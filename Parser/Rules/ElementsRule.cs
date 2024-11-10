using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
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

                            ElementASTNode astNode = new ElementASTNode("",children );
                            
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
            grammar.Add(new Production(ParserConstants.TopElementRule,
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
                    new TerminalExpressionDefinition { TokenType = TokenType.CloseTag },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string elementName = node.GetAttributeForKey<WordToken>("OpenTag", ParserConstants.Token).Lexeme.Replace("<","").Replace(">","");

                        ElementASTNode elementsNode = node.GetAttributeForKey<ElementASTNode>(ParserConstants.ElementsRule, ParserConstants.SyntaxTreeNode);
                        
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ElementASTNode(elementName, elementsNode.Children) { });
                    })
                }
            );
        }
    }
}