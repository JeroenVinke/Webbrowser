using HTMLLexicalAnalyer;
using HTMLParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class HtmlEngine
    {
        private Func<string, string> _filePathResolver = new Func<string, string>(input => input);

        public RenderTreeNode RenderRaw(string html)
        {
            var cssRules = new List<CSSRuleSet>();
            var htmlParser = new HTMLParser.BottomUpParser();

            LexicalAnalyzer htmlAnalyzer = new LexicalAnalyzer(HTMLLexicalAnalyer.LexicalLanguage.GetLanguage(), html);
            htmlParser.Parse(htmlAnalyzer);

            HTMLParser.SyntaxTreeNodes.ElementASTNode node = (HTMLParser.SyntaxTreeNodes.ElementASTNode)htmlParser.TopLevelAST;

            var head = node.Children.First(x => x.ElementName == "head");
            ProcessHead(head);

            RenderTreeNode renderTreeNode = new RenderTreeGenerator(node, cssRules).Generate();

            renderTreeNode.CalculateDimensions();

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
                    cssRules.AddRange(cssRules);
                }
            }
        }

        public void SetFilePathResolver(Func<string, string> resolver)
        {
            _filePathResolver = resolver;
        }
    }
}
