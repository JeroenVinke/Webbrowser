using Compiler;
using Compiler.Common;
using System;

namespace HTMLLexicalAnalyer
{
    public class SimulationResult
    {
        public string Regex { get; set; }
        public int MatchedLength { get; set; }
        public bool Accepted { get; set; }
        public bool IsCandidate { get; set; } = true;
        public bool ExactMatch { get; set; }
        public Func<string, Token> Action { get; internal set; }
    }
}
