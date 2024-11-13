// See https://aka.ms/new-console-template for more information

using Compiler.LexicalAnalyer;
using HTMLLexicalAnalyer;
using HTMLParser.SyntaxTreeNodes;
using Webbrowser.Core;

string htmlInput = "<html><head></head><body background=\"yellow\"><p>this is my text</p><p style=\"display:none\">hidden</p></body></html>";

var htmlParser = new HTMLParser.BottomUpParser();

LexicalAnalyzer htmlAnalyzer = new LexicalAnalyzer(HTMLLexicalAnalyer.LexicalLanguage.GetLanguage(), htmlInput);
htmlParser.Parse(htmlAnalyzer);

HTMLParser.SyntaxTreeNodes.ElementASTNode node = (HTMLParser.SyntaxTreeNodes.ElementASTNode)htmlParser.TopLevelAST;

RenderTreeNode renderTreeNode = new RenderTreeGenerator(node).Generate();

string cssInput = "p { color: red; }";

var cssParser = new CSSParser.BottomUpParser();

var cssAnalyzer = new CSSLexicalAnalyzer.LexicalAnalyzer(CSSLexicalAnalyzer.LexicalLanguage.GetLanguage(), cssInput);

cssParser.Parse(cssAnalyzer);

CSSParser.SyntaxTreeNodes.ElementASTNode cssNode = (CSSParser.SyntaxTreeNodes.ElementASTNode)cssParser.TopLevelAST;

;
