using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternPart
    {
        private PatternBuilder _pattern;
        private PartType _partType;
        private RGG _rGG;
        private PatternPart _nextPatternPart;
        private PatternPart _parentPatternPart;
        private PatternPart _parentEndPatternPart;
        private string _toNodeName;

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

        public PatternPart NonTerminal(string nonterminalName)
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

        public PatternPart Terminal(string terminalPattern)
        {
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.Terminal);
            _nextPatternPart = patternPart;
            patternPart.TerminalPattern = terminalPattern;
            return patternPart;
        }

        public PatternPart Or
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

        public PatternPart GroupStart
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

        public PatternPart NamedGroupStart(string groupName)
        {
            PatternPart patternPart = GroupStart;
            patternPart.Name = groupName;
            return patternPart;
        }

        public PatternPart GroupEnd
        {
            get
            {
                return _parentEndPatternPart;
            }
        }

    }
}
