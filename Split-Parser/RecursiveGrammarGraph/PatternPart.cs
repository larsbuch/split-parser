using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternPart:IPatternPart
    {
        private PatternBuilder _pattern;
        private PartType _partType;
        private RGG _rGG;
        private PatternPart _nextPatternPart;
        private PatternPart _parentPatternPart;
        private PatternPart _parentEndPatternPart;
        private string _toNodeName;
        private int _minRepeats = 1;
        private int? _maxRepeats = 1;


        internal PatternPart(RGG rGG, PatternBuilder pattern, PartType partType)
        {
            _rGG = rGG;
            _pattern = pattern;
            _partType = partType;
            if (partType == PartType.PatternEnd)
            {
                _toNodeName = pattern.Name + " End";
            }
            else
            {
                _toNodeName = pattern.NextNodeName;
            }
        }

        internal string ToNodeName
        {
            get
            {
                return _toNodeName;
            }
        }

        internal string Name { get; set; }

        internal string TerminalPattern { get; set; }

        public IPatternPart NonTerminal(string nonterminalName)
        {
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.NonTerminal);
            _nextPatternPart = patternPart;
            patternPart.Name = nonterminalName;
            return patternPart;
        }

        public void PatternEnd()
        {
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.PatternEnd);
            _nextPatternPart = patternPart;
        }

        internal void Build(string fromNode)
        {
            switch (_partType)
            {
                case PartType.PatternStart:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.PatternStart);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.GroupStart:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.GroupStart);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.GroupEnd:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.GroupEnd);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.Terminal:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.Terminal, TerminalPattern[0]);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.NonTerminal:
                    TransitionType transitionType = TransitionType.NonTerminalNonRecursive;
                    if (Name == _pattern.Name)
                    {
                        transitionType = TransitionType.NonTerminalRecursive;
                    }
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, transitionType, Name);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.PatternEnd:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.PatternEnd);
                    break;
            }
        }

        public IPatternPart Terminal(string terminalPattern)
        {
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.Terminal);
            _nextPatternPart = patternPart;
            patternPart.TerminalPattern = terminalPattern;
            return patternPart;
        }

        public IStartPatternPart Or
        {
            get
            {
                if (_parentPatternPart == null)
                {
                    PatternEnd();
                    return _pattern.StartPattern();
                }
                else
                {
                    _nextPatternPart = _parentEndPatternPart;
                    return _parentPatternPart;
                }
            }
        }

        public IStartPatternPart GroupStart
        {
            get
            {
                PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.GroupStart);
                PatternPart endPatternPart = new PatternPart(_rGG, _pattern, PartType.GroupEnd);
                _nextPatternPart = patternPart;
                _parentPatternPart = patternPart;
                _parentEndPatternPart = endPatternPart;
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
                return _parentEndPatternPart;
            }
        }

        public IPatternPart TerminalEmpty
        {
            get
            {
                PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.Terminal);
                _nextPatternPart = patternPart;
                patternPart.TerminalPattern = string.Empty;
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
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.RepeatNext);
            _nextPatternPart = patternPart;
            _minRepeats = minRepeats;
            _maxRepeats = maxRepeats;
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
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.RepeatPrevious);
            _nextPatternPart = patternPart;
            _minRepeats = minRepeats;
            _maxRepeats = maxRepeats;
            return patternPart;
        }

        #endregion
    }
}
