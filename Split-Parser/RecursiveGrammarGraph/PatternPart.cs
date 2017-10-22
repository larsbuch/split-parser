using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternPart:IPatternPart
    {
        private PatternPart _parentStartPatternPart;
        private PatternPart _parentEndPatternPart;
        private PatternPart _groupStartPatternPart;
        private PatternPart _groupEndPatternPart;
        private List<PatternPart> _childPatternParts;
        private string _toNodeName;
        private string _childToNodeName;
        private string _name;

        internal PatternPart(RGG rGG, PatternBuilder pattern, PartType partType)
        {
            RGG = rGG;
            Pattern = pattern;
            PartType = partType;
            _childPatternParts = new List<PatternPart>();
        }

        public List<PatternPart> ChildPatternParts
        {
            get
            {
                if (PartType != PartType.GroupStart)
                {
                    throw new Exception(string.Format("PartType {0} does not have children", PartType));
                }
                else
                {
                    return _childPatternParts;
                }
            }
        }

        internal string Name
        {
            get
            {
                switch (PartType)
                {
                    case PartType.GroupEnd:
                        return string.Format(Constants.EndPattern, GroupStartPatternPart.Name);
                    default:
                        return _name;
                }
            }
            set
            {
                _name = value;
            }
        }

        internal TerminalPattern TerminalPattern { get; set; }

        internal PatternPart ParentStartPatternPart
        {
            get
            {
                return _parentStartPatternPart;
            }
            set
            {
                if (value != null)
                {
                    if (value.PartType != PartType.GroupStart)
                    {
                        throw new Exception(string.Format("ParentStartPatternPart expected other type than {0}", value.PartType));
                    }
                    else
                    {
                        _parentStartPatternPart = value;
                    }
                }
            }
        }

        internal PatternPart ParentEndPatternPart
        {
            get
            {
                return _parentEndPatternPart;
            }
            set
            {
                if (value != null)
                {
                    if (value.PartType != PartType.GroupEnd)
                    {
                        throw new Exception(string.Format("ParentEndPatternPart expected other type than {0}", value.PartType));
                    }
                    else
                    {
                        _parentEndPatternPart = value;
                    }
                }
            }
        }

        internal PatternPart GroupStartPatternPart
        {
            get
            {
                return _groupStartPatternPart;
            }
            set
            {
                if (value != null)
                {
                    if (value.PartType != PartType.GroupStart)
                    {
                        throw new Exception(string.Format("GroupStartPatternPart expected other type than {0}", value.PartType));
                    }
                    else
                    {
                        _groupStartPatternPart = value;
                    }
                }
            }
        }

        internal PatternPart GroupEndPatternPart
        {
            get
            {
                return _groupEndPatternPart;
            }
            set
            {
                if (value != null)
                {
                    if (value.PartType != PartType.GroupEnd)
                    {
                        throw new Exception(string.Format("GroupEndPatternPart expected other type than {0}", value.PartType));
                    }
                    else
                    {
                        _groupEndPatternPart = value;
                    }
                }
            }
        }

        internal string ToNodeName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_toNodeName))
                {
                    if (PartType == PartType.PatternEnd)
                    {
                        _toNodeName = string.Format(Constants.EndPattern, Pattern.Name);
                    }
                    else if (PartType == PartType.GroupStart)
                    {
                        _toNodeName = string.Format(Constants.GroupStartName, Pattern.NextNodeName, Name);
                    }
                    else if (NextPatternPart.PartType == PartType.GroupEnd)
                    {
                        _toNodeName = NextPatternPart.ChildToNodeName;
                    }
                    else
                    {
                        _toNodeName = Pattern.NextNodeName;
                    }
                }
                return _toNodeName;
            }
        }

        internal string ChildToNodeName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_childToNodeName))
                {
                    _childToNodeName =  string.Format(Constants.GroupEndName, Pattern.NextNodeName, Name);
                }
                return _childToNodeName;
            }
        }

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
                    foreach (PatternPart childPatternPart in ChildPatternParts)
                    {
                        childPatternPart.Build(ToNodeName);
                    }
                    break;
                case PartType.GroupEnd:
                    if (RGG.GetNode(fromNode).TransitionCount == 0)
                    {
                        RGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.GroupEnd, Name);
                        NextPatternPart.Build(ToNodeName);
                    }
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
                if (ParentStartPatternPart == null)
                {
                    PatternEnd();
                    return Pattern.StartPattern();
                }
                else
                {
                    NextPatternPart = ParentEndPatternPart;
                    return ParentStartPatternPart;
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
                    if (GroupEndPatternPart == null)
                    {
                        GroupEndPatternPart = new PatternPart(RGG, Pattern, PartType.GroupEnd);
                        GroupEndPatternPart.GroupStartPatternPart = this;
                        NextPatternPart = GroupEndPatternPart;
                    }
                    patternPart.ParentStartPatternPart = this;
                    patternPart.ParentEndPatternPart = GroupEndPatternPart;
                    ChildPatternParts.Add(patternPart);
                    return patternPart;
                default:
                    NextPatternPart = patternPart;
                    patternPart.ParentStartPatternPart = ParentStartPatternPart;
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
