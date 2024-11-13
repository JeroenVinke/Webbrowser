using HTMLParser.Instances;
using System;

namespace HTMLParser
{
    public abstract class ParsingTableEntry
    {
        public ItemSet ItemSet { get; set; }

        internal abstract string ShortDescription();
    }
}
