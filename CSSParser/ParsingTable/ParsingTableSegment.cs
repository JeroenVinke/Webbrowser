using CSSParser.Instances;
using System.Collections.Generic;

namespace CSSParser
{
    public class ParsingTableSegment
    {
        public ItemSet Set { get; set; }

        public List<ParsingTableEntry> Entries { get; set; } = new List<ParsingTableEntry>();
    }
}
