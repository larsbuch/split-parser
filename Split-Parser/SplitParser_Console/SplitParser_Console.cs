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
            Grammar1(1);
            Grammar2(1);
            /* End Code */

            Console.WriteLine("End: {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            Console.WriteLine("Ended. Press Enter to continue");
            Console.Read();
        }

        private static void Grammar1(int stopAtStep)
        {
            /* Earley parser presentation */
            RGG grammar = new RGG("A calculus", stopAtStep);
            grammar.BuildPattern("Start").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").NonTerminal("Q").NonTerminal("F").Or.NonTerminal("F").PatternEnd();
            grammar.BuildPattern("Q").Terminal("+").Or.Terminal("-").PatternEnd();
            grammar.BuildPattern("F").Terminal("a").PatternEnd();
            grammar.BuildGraph();
            grammar.PrintRGG(string.Format("Step {0}",stopAtStep), "Start");
            // Test input a-a+a
        }

        private static void Grammar2(int stopAtStep)
        {
            /* Efficient Earley Parsing with Regular Right-hand Sides */
            RGG grammar = new RGG("Transducer example", stopAtStep);
            grammar.BuildPattern("Start").Terminal("x").GroupStart.NonTerminal("A").Or.Terminal("y").NonTerminal("B").GroupEnd.PatternEnd();
            grammar.BuildPattern("A").Terminal("y").GroupStart.NonTerminal("B").Or.NonTerminal("C").GroupEnd.PatternEnd();
            grammar.BuildPattern("B").Terminal("w").Terminal("w").Terminal("w").PatternEnd();
            grammar.BuildPattern("C").Terminal("z").PatternEnd();
            grammar.BuildGraph();
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
        }
    }
}
