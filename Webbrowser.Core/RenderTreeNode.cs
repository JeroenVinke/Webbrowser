using System.Drawing;
using Compiler.Parser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class RenderTreeNode
    {
        public string BackgroundColor { get; set; }
        public Rectangle Layout { get; set; }
        public string Type { get; set; }

        public RenderTreeNode(ElementASTNode node)
        {
            Type = node.ElementName;
            Layout = new Rectangle(0, 0, 100, 100);
            BackgroundColor = node.Attributes.GetValueOrDefault("background") ?? "white";
        }

        public List<RenderTreeNode> Children { get; set; } = new ();

        public void AddChild(RenderTreeNode node)
        {
            Children.Add(node);
        }
    }
}
