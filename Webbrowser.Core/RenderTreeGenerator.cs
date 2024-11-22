using HTMLParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class RenderTreeGenerator
    {
        private readonly string[] _visibleElements = { "p", "div", "h1", "h2", "body", "img" };
        private readonly List<CSSRuleSet> _cssRules;
        public ElementASTNode TopLevelNode { get; set; }

        public RenderTreeGenerator(ElementASTNode topLevelNode, List<CSSRuleSet> cssRuleSets)
        {
            TopLevelNode = topLevelNode;
            _cssRules = cssRuleSets;
        }

        public RenderTreeNode Generate()
        {
            return GenerateNode(TopLevelNode);
        }

        private RenderTreeNode GenerateNode(ElementASTNode node)
        {
            var renderNode = new RenderTreeNode(node);
            renderNode.ApplyCssRules(_cssRules);

            foreach (var child in node.Children)
            {
                var childNode = GenerateNode(child);
                if (IsVisibleNode(childNode))
                {
                    renderNode.AddChild(childNode);
                }
            }

            return renderNode;
        }

        private bool IsVisibleNode(RenderTreeNode node)
        {
            if (!_visibleElements.Contains(node.Element))
            {
                return false;
            }

            string? displayAttribute = node.CSSProperties.GetValueOrDefault("display");

            if (displayAttribute == "none")
            {
                return false;
            }

            return true;
        }
    }
}
