using CSSParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class CssEngine
    {
        public List<CSSRuleSet> GetRulesForRawText(string text)
        {
            var cssParser = new CSSParser.BottomUpParser();

            var cssAnalyzer = new CSSLexicalAnalyzer.LexicalAnalyzer(CSSLexicalAnalyzer.LexicalLanguage.GetLanguage(), text);

            cssParser.Parse(cssAnalyzer);

            RuleSetsASTNode cssNode = (RuleSetsASTNode)cssParser.TopLevelAST;

            return new CSSRulesGenerator(cssNode).Generate();
        }
    }
}
