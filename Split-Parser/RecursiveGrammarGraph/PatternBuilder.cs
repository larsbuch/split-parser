using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class PatternBuilder
    {
        private string _patternName;
        private RGG _rGG;
        private List<PatternPart> _startPatternParts;
        private int _nodeCounter = 1;

        internal PatternBuilder(RGG rGG, string patternName)
        {
            _rGG = rGG;
            _patternName = patternName;
            _startPatternParts = new List<PatternPart>();
        }

        internal string Name
        {
            get
            {
                return _patternName;
            }
        }

        internal string NextNodeName
        {
            get
            {
                return string.Format("{0} {1}", Name, _nodeCounter++);
            }
        }

        public PatternPart StartPattern()
        {
            PatternPart patternPart = new PatternPart(_rGG, this, PartType.PatternStart);
            _startPatternParts.Add(patternPart);
            return patternPart;
        }

        internal void Build()
        {
            foreach(PatternPart patternPart in _startPatternParts)
            {
                patternPart.Build(Name);
            }
        }
    }
}
