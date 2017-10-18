using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class TerminalChar
    {
        public TerminalChar(char charInput)
        {
            CharInput = charInput;
            IsCharGroup = false;
        }

        public char CharInput { get; private set; }
        public bool IsCharGroup { get; private set; }

    }
}
