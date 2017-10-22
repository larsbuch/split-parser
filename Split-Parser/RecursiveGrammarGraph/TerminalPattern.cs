using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class TerminalPattern
    {
        private List<TerminalChar> _inputChars;

        public TerminalPattern(string inputString)
        {
            _inputChars = new List<TerminalChar>();
            foreach (char charInput in inputString.ToCharArray())
            {
                TerminalChar inputChar = new TerminalChar(charInput);
                Add(inputChar);
            }
        }

        public int Count
        {
            get
            {
                return _inputChars.Count;
            }
        }

        private void Add(TerminalChar inputChar)
        {
            _inputChars.Add(inputChar);
        }

        public override string ToString()
        {
            if (_inputChars.Count > 0)
            {
                return string.Join("", _inputChars.Select(s => s.ToString()));
            }
            else
            {
                return Constants.Empty;
            }
        }
    }
}
