namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ElementASTNode : SyntaxTreeNode
    {
        //public SymbolTableEntry SymbolTableEntry { get; set; }

        public ElementASTNode(string elementName) : base(SyntaxTreeNodeType.Tag)
        {
            Identifier = elementName;
        }

        public string Identifier { get; internal set; }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
