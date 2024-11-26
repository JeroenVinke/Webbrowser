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
            return "style='" + string.Join(";", GetCSSProperties(node).Select(x => string.Join(":", x.Key, x.Value))) + "'";
        }

        private Dictionary<string, string> GetCSSProperties(RenderTreeNode node)
        {
            var props = new Dictionary<string,string>(node.CSSProperties);

            props["position"] = "static";
            props["left"] = node.Position.X.ToString();
            props["top"] = node.Position.Y.ToString();
            return props;
        }

        private string GetHead()
        {
            return "";
        }
    }
}
