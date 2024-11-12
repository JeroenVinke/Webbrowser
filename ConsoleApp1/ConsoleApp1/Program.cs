// See https://aka.ms/new-console-template for more information

using Compiler.LexicalAnalyer;
using Compiler.Parser;
using Compiler.Parser.SyntaxTreeNodes;
using Webbrowser.Core;
using SyntaxTreeNode = Compiler.RegularExpressionEngine.SyntaxTreeNode;

string input = "<html><head></head><body background=\"yellow\"><p>this is my text</p><p style=\"display:none\">hidden</p></body></html>";

var parser = new BottomUpParser();

LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), input);
parser.Parse(analyzer);

ElementASTNode node = (ElementASTNode)parser.TopLevelAST;

RenderTreeNode renderTreeNode = new RenderTreeGenerator(node).Generate();
;
