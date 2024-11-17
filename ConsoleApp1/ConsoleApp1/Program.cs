using CSSParser.SyntaxTreeNodes;
using HTMLLexicalAnalyer;
using Webbrowser.Core;

var cssParser = new CSSParser.BottomUpParser();

var cssAnalyzer = new CSSLexicalAnalyzer.LexicalAnalyzer(CSSLexicalAnalyzer.LexicalLanguage.GetLanguage(), File.ReadAllText("index.css"));

cssParser.Parse(cssAnalyzer);

RuleSetsASTNode cssNode = (RuleSetsASTNode)cssParser.TopLevelAST;

var cssRules = new CSSRulesGenerator(cssNode).Generate();


var htmlParser = new HTMLParser.BottomUpParser();

LexicalAnalyzer htmlAnalyzer = new LexicalAnalyzer(HTMLLexicalAnalyer.LexicalLanguage.GetLanguage(), File.ReadAllText("index.html"));
htmlParser.Parse(htmlAnalyzer);

HTMLParser.SyntaxTreeNodes.ElementASTNode node = (HTMLParser.SyntaxTreeNodes.ElementASTNode)htmlParser.TopLevelAST;

RenderTreeNode renderTreeNode = new RenderTreeGenerator(node, cssRules).Generate();

;
