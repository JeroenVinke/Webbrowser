using CSSParser.Instances;
using System;

namespace CSSParser
{
    public abstract class ParsingTableEntry
    {
        public ItemSet ItemSet { get; set; }

        internal abstract string ShortDescription();
    }
}
