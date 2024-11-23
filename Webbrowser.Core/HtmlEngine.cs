using HTMLLexicalAnalyer;
using HTMLParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class HtmlEngine
    {
        private Func<string, string> _filePathResolver = input => input;

        private List<CSSRuleSet> _cssRules = new List<CSSRuleSet>();

        public RenderTreeNode RenderRaw(string html)
        {
            var htmlParser = new HTMLParser.BottomUpParser();

            LexicalAnalyzer htmlAnalyzer = new LexicalAnalyzer(HTMLLexicalAnalyer.LexicalLanguage.GetLanguage(), html);
            htmlParser.Parse(htmlAnalyzer);

            HTMLParser.SyntaxTreeNodes.ElementASTNode node = (HTMLParser.SyntaxTreeNodes.ElementASTNode)htmlParser.TopLevelAST;

            var head = node.Children.First(x => x.ElementName == "head");
            ProcessHead(head);

            RenderTreeNode renderTreeNode = new RenderTreeGenerator(node, _cssRules).Generate();

            renderTreeNode.CalculateDimensions();

            renderTreeNode.CalculatePosition();

            return renderTreeNode;
        }

        private void ProcessHead(ElementASTNode headNode)
        {
            foreach (var child in headNode.Children)
            {
                if (child.ElementName == "link")
                {
                    var href = child.Attributes.First(x => x.Key == "href").Value;
                    var cssRules = new CssEngine().GetRulesForRawText(File.ReadAllText(_filePathResolver.Invoke(href)));
                    _cssRules.AddRange(cssRules);
                }
            }
        }

        public void SetFilePathResolver(Func<string, string> resolver)
        {
            _filePathResolver = resolver;
        }
    }
}
