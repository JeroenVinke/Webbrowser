// See https://aka.ms/new-console-template for more information

using Compiler.LexicalAnalyer;
using Compiler.Parser;
using SyntaxTreeNode = Compiler.RegularExpressionEngine.SyntaxTreeNode;

string input = "<html><head></head><body background=\"yellow\"></body></html>";

var parser = new BottomUpParser();

LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), input);
parser.Parse(analyzer);

Compiler.Parser.SyntaxTreeNode node = parser.TopLevelAST;

;
