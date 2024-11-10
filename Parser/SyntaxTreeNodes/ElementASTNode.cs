using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ElementASTNode : SyntaxTreeNode
    {
        public ElementASTNode(string elementName, List<ElementASTNode> children) : base(SyntaxTreeNodeType.Element)
        {
            ElementName = elementName;
            Children = children;
        }

        public List<ElementASTNode> Children { get; set; }

        public string ElementName { get; internal set; }

        public override string ToString()
        {
            return ElementName;
        }
    }
}
