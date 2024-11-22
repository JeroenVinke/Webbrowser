using Webbrowser.Core;

namespace Webbrowser.HTMLRenderer
{
    public class HTMLGenerator
    {
        private readonly RenderTreeNode _renderTreeNode;

        public HTMLGenerator(RenderTreeNode renderTreeNode)
        {
            _renderTreeNode = renderTreeNode;
        }

        public string GenerateHTML()
        {
            var html = "<html><head>";

            html += GetHead();

            html += "</head><body>";

            html += GetBody();

            html += "</body></html>";

            return html;
        }

        private string GetBody()
        {
            return GetHTML(_renderTreeNode);
        }

        private string GetHTML(RenderTreeNode node)
        {
            if (node.Element == "img")
            {
                return "<img src='" + node.Attributes["src"] + "' " + GetStyles(node) + "/>";
            }
            string result = "<" + node.Element + " " + GetStyles(node) + ">";

            result += node.InnerText;

            foreach (var child in node.Children)
            {
                result += GetHTML(child);
            }

            result += "</" + node.Element + ">";

            return result;
        }

        private string GetStyles(RenderTreeNode node)
        {
            return "style='" + string.Join(";", node.CSSProperties.Select(x => string.Join(":", x.Key, x.Value))) + "'";
        }

        private string GetHead()
        {
            return "";
        }
    }
}
