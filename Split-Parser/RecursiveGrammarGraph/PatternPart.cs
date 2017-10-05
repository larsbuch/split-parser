using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternPart
    {
        private Pattern _pattern;
        private PartType _partType;
        private RGG _rGG;
        private PatternPart _nextPatternPart;
        private string _toNodeName;

        internal PatternPart(RGG rGG, Pattern pattern, PartType partType)
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

        internal string NonTerminalName { get; set; }

        internal string TerminalPattern { get; set; }

        public PatternPart NonTerminal(string nonterminalName)
        {
            PatternPart patternPart = new PatternPart(_rGG, _pattern, PartType.NonTerminal);
            _nextPatternPart = patternPart;
            patternPart.NonTerminalName = nonterminalName;
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
                case PartType.Terminal:
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, TransitionType.Terminal, TerminalPattern[0]);
                    _nextPatternPart.Build(ToNodeName);
                    break;
                case PartType.NonTerminal:
                    TransitionType transitionType = TransitionType.NonTerminalNonRecursive;
                    if (NonTerminalName == _pattern.Name)
                    {
                        transitionType = TransitionType.NonTerminalRecursive;
                    }
                    _rGG.CreateRGGTransition(fromNode, ToNodeName, transitionType, NonTerminalName);
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
                PatternEnd();
                return _pattern.StartPattern();
            }
        }
    }
}
