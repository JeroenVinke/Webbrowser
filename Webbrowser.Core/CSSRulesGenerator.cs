using CSSParser.SyntaxTreeNodes;

namespace Webbrowser.Core
{
    public class CSSRulesGenerator
    {
        private readonly RuleSetsASTNode _node;

        public CSSRulesGenerator(RuleSetsASTNode node)
        {
            _node = node;
        }

        public List<CSSRuleSet> Generate()
        {
            var result = new List<CSSRuleSet>();

            foreach (RuleSetASTNode node in _node.Children)
            {
                var selectors = ParseSelector(node.Selector);

                foreach (var selector in selectors)
                {
                    result.Add(new CSSRuleSet() {  Selector = selector, Properties = node.Declarations.Select(ToCSSProperty).ToList()});
                }
            }

            return result;
        }

        private CSSProperty ToCSSProperty(DeclarationASTNode arg)
        {
            return new CSSProperty()
            {
                Property = arg.Property,
                Value = arg.Value
            };
        }


        private List<CssSelector> ParseSelector(string selector)
        {
            var result = new List<CssSelector>();
            var parts = selector.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.StartsWith("#"))
                {
                    result.Add(new CssSelector
                    {
                        Type = SelectorType.Id,
                        Value = part.Substring(1)
                    });
                }
                else if (part.StartsWith("."))
                {
                    result.Add(new CssSelector
                    {
                        Type = SelectorType.Class,
                        Value = part.Substring(1)
                    });
                }
                else
                {
                    result.Add(new CssSelector
                    {
                        Type = SelectorType.Element,
                        Value = part
                    });
                }
            }

            return result;
        }
    }
}
