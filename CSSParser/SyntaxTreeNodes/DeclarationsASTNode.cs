using System.Collections.Generic;

namespace CSSParser.SyntaxTreeNodes
{
    public class DeclarationsASTNode : SyntaxTreeNode
    {
        public List<DeclarationASTNode> Children { get; set; } = new List<DeclarationASTNode>();

        public DeclarationsASTNode() : base(SyntaxTreeNodeType.Element)
        {
        }

        public override string ToString()
        {
            return nameof(DeclarationsASTNode);
        }
    }
}
