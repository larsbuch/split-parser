using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternPart:IPatternPart
    {

        internal PatternPart(RGG rGG, PatternBuilder pattern, PartType partType)
        {
            RGG = rGG;
            Pattern = pattern;
            PartType = partType;
            if (partType == PartType.PatternEnd)
            {
                ToNodeName = pattern.Name + " End";
            }
            else
            {
                ToNodeName = pattern.NextNodeName;
            }
        }

        internal string Name { get; set; }

        internal TerminalPattern TerminalPattern { get; set; }

        internal PatternPart ParentPatternPart { get; set; }

        internal PatternPart ParentEndPatternPart { get; set; }

        internal PatternPart EndPatternPart { get; set; }

        internal string ToNodeName { get; private set; }

        internal int MinRepeats { get; set; }

        internal int? MaxRepeats { get; set; }

        private PatternPart NextPatternPart { get; set; }

        private PatternBuilder Pattern { get; set; }

        private RGG RGG { get; set; }

        private PartType PartType { get; set; }

        public IPatternPart NonTerminal(string nonterminalName)
        {
            PatternPart patternPart = NewPatternPart(PartType.NonTerminal);
            patternPart.Name = nonterminalName;
            return patternPart;
        }

        public void PatternEnd()
        {
            NewPatternPart(PartType.PatternEnd);
        }

        internal void Build(string fromNode)
        {
            switch (PartType)
            {
                case PartType.PatternStart:
                    RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.PatternStart);
                    NextPatternPart.Build(ToNodeName);
                    break;
                case PartType.GroupStart:
                    RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.GroupStart, Name);
                    NextPatternPart.Build(ToNodeName);
                    break;
                case PartType.GroupEnd:
                    RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.GroupEnd);
                    NextPatternPart.Build(ToNodeName);
                    break;
                case PartType.Terminal:
                    if (TerminalPattern == null)
                    {
                        TerminalPattern = new TerminalPattern(string.Empty);
                    }
                    RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.Terminal, TerminalPattern);
                    NextPatternPart.Build(ToNodeName);
                    break;
                case PartType.NonTerminal:
                    TransitionType transitionType = TransitionType.NonTerminalNonRecursive;
                    if (Name == Pattern.Name)
                    {
                        transitionType = TransitionType.NonTerminalRecursive;
                    }
                    RGG.CreateRGGTransition(fromNode, ToNodeName, transitionType, Name);
                    NextPatternPart.Build(ToNodeName);
                    break;
                case PartType.PatternEnd:
                    RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.PatternEnd);
                    break;
            }
        }

        public IPatternPart Terminal(string terminalPattern)
        {
            PatternPart patternPart = NewPatternPart(PartType.Terminal);
            patternPart.TerminalPattern = ConvertStringToPattern(terminalPattern);
            return patternPart;
        }

        private TerminalPattern ConvertStringToPattern(string terminalPattern)
        {
            return new TerminalPattern(terminalPattern);
        }

        public IStartPatternPart Or
        {
            get
            {
                if (ParentPatternPart == null)
                {
                    PatternEnd();
                    return Pattern.StartPattern();
                }
                else
                {
                    NextPatternPart = ParentEndPatternPart;
                    return ParentPatternPart;
                }
            }
        }

        public IStartPatternPart GroupStart
        {
            get
            {
                PatternPart patternPart = NewPatternPart(PartType.GroupStart);
                patternPart.Name = Constants.UnnamedGroupStart;
                return patternPart;
            }
        }

        public IStartPatternPart NamedGroupStart(string groupName)
        {
            PatternPart patternPart = GroupStart as PatternPart;
            patternPart.Name = groupName;
            return patternPart;
        }

        public IPatternPart GroupEnd
        {
            get
            {
                if (ParentEndPatternPart != null)
                {
                    NextPatternPart = ParentEndPatternPart;
                    return ParentEndPatternPart;
                }
                else
                {
                    throw new Exception("GroupEnd without a group start");
                }
            }
        }

        public IPatternPart TerminalEmpty
        {
            get
            {
                PatternPart patternPart = NewPatternPart(PartType.Terminal);
                patternPart.TerminalPattern = ConvertStringToPattern(string.Empty);
                return patternPart;
            }
        }

        private PatternPart NewPatternPart(PartType newPattrenPartType)
        {
            PatternPart patternPart = new PatternPart(RGG, Pattern, newPattrenPartType);
            switch (PartType)
            {
                case PartType.PatternEnd:
                    NextPatternPart = patternPart;
                    return null;
                case PartType.GroupStart:
                    EndPatternPart = new PatternPart(RGG, Pattern, PartType.GroupEnd);
                    NextPatternPart = patternPart;
                    patternPart.ParentPatternPart = patternPart;
                    patternPart.ParentEndPatternPart = EndPatternPart;
                    return patternPart;
                default:
                    NextPatternPart = patternPart;
                    patternPart.ParentPatternPart = ParentPatternPart;
                    patternPart.ParentEndPatternPart = ParentEndPatternPart;
                    return patternPart;
            }
        }

        #region Repetition

        public IStartPatternPart OptionalNext
        {
            get
            {
                return RepeatNext(0, 1);
            }
        }

        public IStartPatternPart RepeatNextZeroOrMore
        {
            get
            {
                return RepeatNext(0, null);
            }
        }

        public IStartPatternPart RepeatNextOnceOrMore
        {
            get
            {
                return RepeatNext(1, null);
            }
        }

        public IStartPatternPart RepeatNext(int repeats)
        {
            return RepeatNext(repeats, repeats);
        }

        public IStartPatternPart RepeatNext(int minRepeats, int? maxRepeats)
        {
            PatternPart patternPart = NewPatternPart(PartType.RepeatNext);
            patternPart.MinRepeats = minRepeats;
            patternPart.MaxRepeats = maxRepeats;
            return patternPart;
        }
        public IPatternPart OptionalPrevious
        {
            get
            {
                return RepeatPrevious(0, 1);
            }
        }

        public IPatternPart RepeatPreviousZeroOrMore
        {
            get
            {
                return RepeatPrevious(0, null);
            }
        }

        public IPatternPart RepeatPreviousOnceOrMore
        {
            get
            {
                return RepeatPrevious(1, null);
            }
        }

        public IPatternPart RepeatPrevious(int repeats)
        {
            return RepeatPrevious(repeats, repeats);
        }

        public IPatternPart RepeatPrevious(int minRepeats, int? maxRepeats)
        {
            PatternPart patternPart = NewPatternPart(PartType.RepeatPrevious);
            patternPart.MinRepeats = minRepeats;
            patternPart.MaxRepeats = maxRepeats;
            return patternPart;
        }

        #endregion
    }
}
