using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public enum TransitionType
    {
        PatternStart,
        PatternEnd,
        Pop,
        Push,
        Complete,
        Lookup,
        Terminal,
        NonTerminalNonRecursive,
        NonTerminalRecursive
    }
}
