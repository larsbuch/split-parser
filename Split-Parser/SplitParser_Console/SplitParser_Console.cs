using RecursiveGrammarGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitParser_Console
{
    class SplitParser_Console
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Start: {0}", stopwatch.ElapsedMilliseconds);

            /* Code */
            RGG grammar = new RGG("A calculus", 1);
            grammar.BuildPattern("Start").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").NonTerminal("Q").NonTerminal("F").Or.NonTerminal("F").PatternEnd();
            grammar.BuildPattern("Q").Terminal("+").Or.Terminal("-").PatternEnd();
            grammar.BuildPattern("F").Terminal("a").PatternEnd();
            grammar.BuildGraph();
            grammar.PrintRGG("Step 1", "Start");
            /* End Code */

            Console.WriteLine("End: {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            Console.WriteLine("Ended. Press Enter to continue");
            Console.Read();
        }
    }
}
