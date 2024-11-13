using HTMLParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class RenderTreeGenerator
    {
        private readonly string[] _visibleElements = { "p", "div", "h1", "h2", "body" };
        public ElementASTNode TopLevelNode { get; set; }

        public RenderTreeGenerator(ElementASTNode topLevelNode)
        {
            TopLevelNode = topLevelNode;
        }

        public RenderTreeNode Generate()
        {
            return GenerateNode(TopLevelNode);
        }

        private RenderTreeNode GenerateNode(ElementASTNode node)
        {
            var renderNode = new RenderTreeNode(node);

            foreach (var child in node.Children)
            {
                if (IsVisibleNode(child))
                {
                    renderNode.AddChild(GenerateNode(child));
                }
            }

            return renderNode;
        }

        private bool IsVisibleNode(ElementASTNode node)
        {
            if (!_visibleElements.Contains(node.ElementName))
            {
                return false;
            }

            //string? displayAttribute = node.Attributes.GetValueOrDefault("display");

            //if (displayAttribute == "none")
            //{
            //    return false;
            //}

            return true;
        }
    }
}
