using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler.Parser
{
    public class HtmlElement
    {
        public string Name { get; private set; }
        public Dictionary<string, string> Attributes { get; private set; }

        public HtmlElement(string rawElement)
        {
            Attributes = new Dictionary<string, string>();
            ParseElement(rawElement);
        }

        private void ParseElement(string tag)
        {
            var pattern = @"<(\w+)\s*|(\w+)\s*=\s*""([^""]*)""";

            var matches = Regex.Matches(tag, pattern);

            bool firstMatch = true;
            foreach (Match match in matches)
            {
                if (firstMatch)
                {
                    Name = match.Groups[1].Value;
                    firstMatch = false;
                }
                else if (match.Groups[2].Success && match.Groups[3].Success)
                {
                    string attrName = match.Groups[2].Value;
                    string attrValue = match.Groups[3].Value;
                    Attributes[attrName] = attrValue;
                }
            }
        }

        public override string ToString()
        {
            var attrs = string.Join(", ", Attributes);
            return $"Element Name: {Name}, Attributes: {attrs}";
        }
    }

}
