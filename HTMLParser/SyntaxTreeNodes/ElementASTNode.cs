using System.Collections.Generic;

namespace HTMLParser.SyntaxTreeNodes
{
    public class ElementASTNode : SyntaxTreeNode
    {
        public Dictionary<string, string> Attributes { get; set; }

        public List<ElementASTNode> Children { get; set; }

        public string ElementName { get; internal set; }
        public string InnerText { get; set; }

        public ElementASTNode(string elementName, Dictionary<string, string> htmlElementAttributes,
            List<ElementASTNode> children, string innerText = "") : base(SyntaxTreeNodeType.Element)
        {
            ElementName = elementName;
            Children = children;
            Attributes = htmlElementAttributes;
            InnerText = innerText;
        }



        public ElementASTNode(string elementName,
            List<ElementASTNode> children) : this(elementName, new Dictionary<string, string>(), children)
        {
            ElementName = elementName;
            Children = children;
        }

        public override string ToString()
        {
            return ElementName;
        }
    }
}
