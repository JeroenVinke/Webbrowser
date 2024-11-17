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
                            var children = new List<RuleSetASTNode>();

                            children.Add(node.GetAttributeForKey<RuleSetASTNode>(ParserConstants.RuleSetRule, ParserConstants.SyntaxTreeNode));
                            children.AddRange(node.GetAttributeForKey<RuleSetsASTNode>(ParserConstants.RuleSetsRule, ParserConstants.SyntaxTreeNode).Children);

                            RuleSetsASTNode astNode = new RuleSetsASTNode();
                            astNode.Children = children;
                            
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
                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new RuleSetsASTNode());
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
            grammar.Add(new Production(ParserConstants.DeclarationsRule,
                new List<SubProduction>
                {
                    new SubProduction(new List<ExpressionDefinition>()
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.DeclarationRule },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.DeclarationsRule },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            var children = new List<DeclarationASTNode>();

                            children.Add(node.GetAttributeForKey<DeclarationASTNode>(ParserConstants.DeclarationRule, ParserConstants.SyntaxTreeNode));
                            children.AddRange(node.GetAttributeForKey<DeclarationsASTNode>(ParserConstants.DeclarationsRule, ParserConstants.SyntaxTreeNode).Children);

                            DeclarationsASTNode astNode = new DeclarationsASTNode();
                            astNode.Children = children;

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
                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new DeclarationsASTNode());
                            })
                        }
                    )
                }
            ));
            grammar.Add(new Production(ParserConstants.DeclarationRule,
                new List<SubProduction>
                {
                    DeclarationRule()
                }
            ));
        }

        private static SubProduction DeclarationRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier, Key = "Property" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Colon },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier, Key = "Value" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string property = node.GetAttributeForKey<WordToken>("Property", ParserConstants.Token).Lexeme;
                        string value = node.GetAttributeForKey<WordToken>("Value", ParserConstants.Token).Lexeme;
                        ;
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new DeclarationASTNode(property, value) { });
                    })
                }
            );
        }


        private static SubProduction RuleSetRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier, Key = "Selector" },
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketOpen },
                    new NonTerminalExpressionDefinition() { Identifier = ParserConstants.DeclarationsRule },
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketClosed },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string selector = node.GetAttributeForKey<WordToken>("Selector", ParserConstants.Token).Lexeme;
                        List<DeclarationASTNode> declarations = node.GetAttributeForKey<DeclarationsASTNode>(ParserConstants.DeclarationsRule, ParserConstants.SyntaxTreeNode).Children;
                        
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new RuleSetASTNode(selector, declarations) { });
                    })
                }
            );
        }
    }
}