
using System.Collections.Generic;

namespace CSSParser.SyntaxTreeNodes
{
    public class RuleSetASTNode : SyntaxTreeNode
    {
        public string Selector { get; set; }
        public List<DeclarationASTNode> Declarations { get; set; }

        public RuleSetASTNode(string selector, List<DeclarationASTNode> declarations) : base(SyntaxTreeNodeType.Element)
        {
            Selector = selector;
            Declarations = declarations;
        }

        public override string ToString()
        {
            return nameof(RuleSetASTNode);
        }
    }
}
