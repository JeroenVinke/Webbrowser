﻿using System.Collections.Generic;
using System.Linq;

namespace HTMLParser
{
    public class Production : List<SubProduction>
    {

        public string Identifier { get; set; }

        public Production(string identifier, List<SubProduction> list) : base(list)
        {
            Identifier = identifier;

            ForEach(x => x.Production = this);
        }

        public Production(string identifier, SubProduction subProduction) : this(identifier, new List<SubProduction>() { subProduction })
        {
        }

        public override string ToString()
        {
            return Identifier + " -> " + string.Join("\r\n\t\t | ", this.Select(x => x.ToString()));
        }
    }
}