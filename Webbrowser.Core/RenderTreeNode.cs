using System.Data;
using System.Drawing;
using Compiler.RegularExpressionEngine;
using HTMLParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class RenderTreeNode
    {
        public Rectangle Layout { get; set; }
        public string Type { get; set; }

        public string Element { get; set; }
        public string Id { get; set; }
        public string Class { get; set; }

        public Dictionary<string, string> CSSProperties = new Dictionary<string, string>()
        {
            { "background-color", "white" },
            { "color", "black" }
        };
        private readonly ElementASTNode _node;

        public RenderTreeNode(ElementASTNode node)
        {
            _node = node;
            Class = node.Attributes.GetValueOrDefault("class") ?? "";
            Id = node.Attributes.GetValueOrDefault("id") ?? "";
            Element = node.ElementName;
            Type = node.ElementName;
            Layout = new Rectangle(0, 0, 100, 100);
        }

        public List<RenderTreeNode> Children { get; set; } = new ();

        public void AddChild(RenderTreeNode node)
        {
            Children.Add(node);
        }

        public void ApplyCssRules(List<CSSRuleSet> cssRules)
        {
            ApplyHtmlStyles();

            var relevantSets = cssRules.Where(x => x.Selector.Type == SelectorType.Id && x.Selector.Value == Id)
                .Union(cssRules.Where(x => x.Selector.Type == SelectorType.Class && x.Selector.Value == Class))
                .Union(cssRules.Where(x => x.Selector.Type == SelectorType.Element && x.Selector.Value == Element));

            foreach (var set in relevantSets)
            {
                foreach (var property in set.Properties)
                {
                    CSSProperties[property.Property] = property.Value;
                }
            }

            ApplyInlineStyles();
        }

        private void ApplyInlineStyles()
        {
            var style = _node.Attributes.GetValueOrDefault("style") ?? "";

            if (!string.IsNullOrEmpty(style))
            {
                var styleProperties = ParseCssStyles(style);

                foreach (var styleProperty in styleProperties)
                {
                    CSSProperties[styleProperty.Key] = styleProperty.Value;
                }
            }
        }

        private static List<KeyValuePair<string, string>> ParseCssStyles(string styles)
        {
            var result = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(styles))
                return result;

            // Split the styles by semicolon
            var declarations = styles.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var declaration in declarations)
            {
                // Split each declaration by colon
                var parts = declaration.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    result.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return result;
        }


        private void ApplyHtmlStyles()
        {
            string background = _node.Attributes.GetValueOrDefault("background") ?? "white";
            if (!string.IsNullOrEmpty(background))
            {
                CSSProperties["background-color"] = background;
            }

            string color = _node.Attributes.GetValueOrDefault("color") ?? "white";
            if (!string.IsNullOrEmpty(color))
            {
                CSSProperties["color"] = color;
            }
        }
    }
}
