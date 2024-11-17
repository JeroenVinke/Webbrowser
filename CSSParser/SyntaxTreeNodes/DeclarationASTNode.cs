
namespace CSSParser.SyntaxTreeNodes
{
    public class DeclarationASTNode : SyntaxTreeNode
    {
        public string Value { get; set; }
        public string Property { get; set; }

        public DeclarationASTNode(string property, string value) : base(SyntaxTreeNodeType.Element)
        {
            Property = property;
            Value = value;
        }

        public override string ToString()
        {
            return nameof(DeclarationASTNode);
        }
    }
}
