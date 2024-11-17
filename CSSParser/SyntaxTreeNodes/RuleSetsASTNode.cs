using System.Collections.Generic;

namespace CSSParser.SyntaxTreeNodes
{
    public class RuleSetsASTNode : SyntaxTreeNode
    {
        public List<RuleSetASTNode> Children { get; set; } = new List<RuleSetASTNode>();

        public RuleSetsASTNode() : base(SyntaxTreeNodeType.Element)
        {
        }

        public override string ToString()
        {
            return nameof(RuleSetsASTNode);
        }
    }
}
