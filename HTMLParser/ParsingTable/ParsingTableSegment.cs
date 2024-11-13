using HTMLParser.Instances;
using System.Collections.Generic;

namespace HTMLParser
{
    public class ParsingTableSegment
    {
        public ItemSet Set { get; set; }

        public List<ParsingTableEntry> Entries { get; set; } = new List<ParsingTableEntry>();
    }
}
