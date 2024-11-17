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
        public int? Width => GetSizeInPixels("width");
        public int? Height => GetSizeInPixels("height");
        public int PaddingLeft => GetSizeInPixels("padding-left").Value;
        public int PaddingRight => GetSizeInPixels("padding-right").Value;
        public int PaddingTop => GetSizeInPixels("padding-top").Value;
        public int PaddingBottom => GetSizeInPixels("padding-bottom").Value;
        public int MarginLeft => GetSizeInPixels("margin-left").Value;
        public int MarginRight => GetSizeInPixels("margin-right").Value;
        public int MarginTop => GetSizeInPixels("margin-top").Value;
        public int MarginBottom => GetSizeInPixels("margin-bottom").Value; 
        public int BorderLeft => GetSizeInPixels("border-left").Value;
        public int BorderRight => GetSizeInPixels("border-right").Value;
        public int BorderTop => GetSizeInPixels("border-top").Value;
        public int BorderBottom => GetSizeInPixels("border-bottom").Value;
        public int ContentWidth { get; set; }
        public int ContentHeight { get; set; }

        public int CalculatedWidth { get; private set; }
        public int CalculatedHeight { get; private set; }

        public Dictionary<string, string> CSSProperties = new Dictionary<string, string>()
        {
            { "background-color", "white" },
            { "color", "black" },
            { "padding-left", "0px" },
            { "padding-right", "0px" },
            { "padding-top", "0px" },
            { "padding-bottom", "0px" },
            { "margin-left", "0px" },
            { "margin-right", "0px" },
            { "margin-top", "0px" },
            { "margin-bottom", "0px" },
            { "border-left", "0px" },
            { "border-right", "0px" },
            { "border-top", "0px" },
            { "border-bottom", "0px" },
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

        public RenderTreeNode Parent { get; set; }

        public void AddChild(RenderTreeNode node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public void CalculateDimensions()
        {
            // Calculate content width
            if (Width.HasValue)
            {
                ContentWidth = Width.Value;
            }
            else
            {
                ContentWidth = Parent?.ContentWidth ?? 0; // Default to parent width if auto
            }

            // Add padding, border, and margin to calculate total width
            CalculatedWidth = ContentWidth +
                              PaddingLeft + PaddingRight +
                              BorderLeft + BorderRight +
                              MarginLeft + MarginRight;

            // Calculate content height
            if (Height.HasValue)
            {
                ContentHeight = Height.Value;
            }
            else
            {
                // Height calculation for auto can be based on content or children
                ContentHeight = Children.Any()
                    ? Children.Sum(c => c.CalculatedHeight) // Sum of children heights for vertical stacking
                    : 0; // Default to content size for inline or empty block

            }

            // Add padding, border, and margin to calculate total height
            CalculatedHeight = ContentHeight +
                               PaddingTop + PaddingBottom +
                               BorderTop + BorderBottom +
                               MarginTop + MarginBottom;

            foreach (var child in Children)
            {
                child.CalculateDimensions();
            }
        }

        private int? GetSizeInPixels(string property)
        {
            if (!CSSProperties.ContainsKey(property))
            {
                return null;
            }
            return int.Parse(CSSProperties[property].Split("px")[0]);
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
