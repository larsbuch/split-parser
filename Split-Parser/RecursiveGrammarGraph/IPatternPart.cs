using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public interface IStartPatternPart
    {
        IPatternPart Terminal(string terminalPattern);
        IPatternPart TerminalEmpty { get; }
        IPatternPart NonTerminal(string nonterminalName);
        IStartPatternPart GroupStart { get; }
        IStartPatternPart NamedGroupStart(string groupName);
        IStartPatternPart OptionalNext { get; }
        IStartPatternPart RepeatNextZeroOrMore { get; }
        IStartPatternPart RepeatNextOnceOrMore { get; }
        IStartPatternPart RepeatNext(int repeats);
        IStartPatternPart RepeatNext(int minRepeats, int? maxRepeats);
    }

    public interface IPatternPart : IStartPatternPart
    {
        IStartPatternPart Or { get; }
        IPatternPart GroupEnd { get; }
        void PatternEnd();
        IPatternPart OptionalPrevious { get; }
        IPatternPart RepeatPreviousZeroOrMore { get; }
        IPatternPart RepeatPreviousOnceOrMore { get; }
        IPatternPart RepeatPrevious(int repeats);
        IPatternPart RepeatPrevious(int minRepeats, int? maxRepeats);
    }
}

