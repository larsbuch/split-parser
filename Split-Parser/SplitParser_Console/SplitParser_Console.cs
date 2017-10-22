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
            int counter = 1;
            int maxCounter = 1;
            while (counter <= maxCounter)
            {
                //Grammar1(counter);
                //Grammar2(counter);
                //Grammar3(counter);
                //Grammar4(counter);
                //Grammar5(counter);
                //Grammar6(counter);
                //Grammar7(counter);
                //Grammar8(counter);
                //Grammar9(counter);
                //Grammar10(counter);
                //Grammar11(counter);
                //Grammar12(counter);
                //Grammar13(counter);
                //Grammar14(counter);
                //Grammar15(counter);
                //Grammar16(counter);
                //Grammar17(counter);
                //Grammar18(counter);
                //Grammar19(counter);
                //Grammar20(counter);
                //Grammar21(counter);
                //Grammar22(counter);
                //Grammar23(counter);
                //Grammar24(counter);
                //Grammar25(counter);
                //Grammar26(counter);
                Grammar27(counter);
                //Grammar28(counter);
                counter += 1;
            }
            /* End Code */

            Console.WriteLine("End: {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            Console.WriteLine("Ended. Press Enter to continue");
            Console.Read();
        }

        private static void Grammar1(int stopAtStep)
        {
            /* Efficient Earley Parsing with Regular Right-hand Sides */
            RGG grammar = new RGG("Transducer example");
            grammar.BuildPattern("Start").Terminal("x").GroupStart.NonTerminal("A").Or.Terminal("y").NonTerminal("B").GroupEnd.PatternEnd();
            grammar.BuildPattern("A").Terminal("y").GroupStart.NonTerminal("B").Or.NonTerminal("C").GroupEnd.PatternEnd();
            grammar.BuildPattern("B").Terminal("w").Terminal("w").Terminal("w").PatternEnd();
            grammar.BuildPattern("C").Terminal("z").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match 
            // Test input: Non-Match 
        }

        private static void Grammar2(int stopAtStep)
        {
            /* Earley parser presentation */
            RGG grammar = new RGG("A calculus");
            grammar.BuildPattern("Start").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").NonTerminal("Q").NonTerminal("F").Or.NonTerminal("F").PatternEnd();
            grammar.BuildPattern("Q").Terminal("+").Or.Terminal("-").PatternEnd();
            grammar.BuildPattern("F").Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}",stopAtStep), "Start");
            // Test input: Match a-a+a and a+a-a
            // Test input: Non-Match aa+a, a-aa and a--a+a
        }

        private static void Grammar3(int stopAtStep)
        {
            /* Earley parser presentation */
            RGG grammar = new RGG("Worst case");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("S").NonTerminal("S").Or.Terminal("x").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match x, xx, xxxx
            // Test input: Non-Match xxx, xxxxx
        }

        private static void Grammar4(int stopAtStep)
        {
            /* Earley parser presentation */
            RGG grammar = new RGG("A calculus with empty");
            grammar.BuildPattern("Start").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").NonTerminal("Q").NonTerminal("F").Or.NonTerminal("F").PatternEnd();
            grammar.BuildPattern("Q").Terminal("*").Or.Terminal("/").Or.TerminalEmpty.PatternEnd();
            grammar.BuildPattern("F").Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match aa/a, aa*a, a/aa and a/a*a
            // Test input: Non-Match a**a/a
        }

        private static void Grammar5(int stopAtStep)
        {
            /* Earley parser explained */
            RGG grammar = new RGG("EBNF");
            grammar.BuildPattern("Start").NonTerminal("Sum").PatternEnd();
            grammar.BuildPattern("Sum").NonTerminal("Sum").GroupStart.Terminal("+").Or.Terminal("-").GroupEnd.NonTerminal("Product").Or.NonTerminal("Product").PatternEnd();
            grammar.BuildPattern("Product").NonTerminal("Product").GroupStart.Terminal("*").Or.Terminal("/").GroupEnd.NonTerminal("Factor").Or.NonTerminal("Factor").PatternEnd();
            grammar.BuildPattern("Factor").Terminal("(").NonTerminal("Sum").Terminal(")").Or.NonTerminal("Number").PatternEnd();
            grammar.BuildPattern("Number").Terminal("[0-9]+").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match 1+(2*4-4)
            // Test input: Non-Match 
        }

        private static void Grammar6(int stopAtStep)
        {
            /* Earley parser explained */
            RGG grammar = new RGG("Restricted Syntax");
            grammar.BuildPattern("Start").NonTerminal("Sum").PatternEnd();
            grammar.BuildPattern("Sum").NonTerminal("Sum").GroupStart.Terminal("+").Or.Terminal("-").GroupEnd.NonTerminal("Product").PatternEnd();
            grammar.BuildPattern("Sum").NonTerminal("Product").PatternEnd();
            grammar.BuildPattern("Product").NonTerminal("Product").GroupStart.Terminal("*").Or.Terminal("/").GroupEnd.NonTerminal("Factor").PatternEnd();
            grammar.BuildPattern("Product").NonTerminal("Factor").PatternEnd();
            grammar.BuildPattern("Factor").Terminal("(").NonTerminal("Sum").Terminal(")").PatternEnd();
            grammar.BuildPattern("Factor").NonTerminal("Number").PatternEnd();
            grammar.BuildPattern("Number").Terminal("[0-9]").NonTerminal("Number").PatternEnd();
            grammar.BuildPattern("Number").Terminal("[0-9]").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match 1+(2*4-4)
            // Test input: Non-Match 
        }

        private static void Grammar7(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("AE");
            grammar.BuildPattern("Start").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("T").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").Terminal("+").NonTerminal("T").PatternEnd();
            grammar.BuildPattern("T").NonTerminal("P").PatternEnd();
            grammar.BuildPattern("T").NonTerminal("T").Terminal("*").NonTerminal("P").PatternEnd();
            grammar.BuildPattern("P").Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match a+a*a
            // Test input: Non-Match 
        }

        private static void Grammar8(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("UBDA");
            grammar.BuildPattern("Start").NonTerminal("A").PatternEnd();
            grammar.BuildPattern("A").Terminal("x").Or.NonTerminal("A").NonTerminal("A").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match x, xx, xxxx
            // Test input: Non-Match xxx, xxxxx
        }

        private static void Grammar9(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("BK");
            grammar.BuildPattern("Start").NonTerminal("K").PatternEnd();
            grammar.BuildPattern("K").TerminalEmpty.Or.NonTerminal("K").NonTerminal("J").PatternEnd();
            grammar.BuildPattern("J").NonTerminal("F").Or.NonTerminal("I").PatternEnd();
            grammar.BuildPattern("F").Terminal("x").PatternEnd();
            grammar.BuildPattern("I").Terminal("x").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match Empty, x, xx, xxx, xxxx
            // Test input: Non-Match 
        }

        private static void Grammar10(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("PAL");
            grammar.BuildPattern("Start").NonTerminal("A").PatternEnd();
            grammar.BuildPattern("A").Terminal("x").Or.Terminal("x").NonTerminal("A").Terminal("x").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match Unequal number of X
            // Test input: Non-Match Equal number of x
        }

        private static void Grammar11(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("G1");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").Terminal("b").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").Or.NonTerminal("A").Terminal("b").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match abbbbb
            // Test input: Non-Match aabb
        }

        private static void Grammar12(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("G2");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").Terminal("a").NonTerminal("B").PatternEnd();
            grammar.BuildPattern("B").Terminal("a").NonTerminal("B").Or.Terminal("b").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match aaaab
            // Test input: Non-Match aabb
        }

        private static void Grammar13(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("G3");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").Terminal("a").Terminal("b").Or.Terminal("a").NonTerminal("S").Terminal("b").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match number of a followed by same number of b fx aabb
            // Test input: Non-Match aaabb
        }

        private static void Grammar14(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("G4");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("B").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").Or.NonTerminal("A").Terminal("b").PatternEnd();
            grammar.BuildPattern("B").Terminal("b").Terminal("c").Or.Terminal("b").NonTerminal("B").Or.NonTerminal("B").Terminal("d").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match a + number of b + cd
            // Test input: Non-Match abbbccd
        }

        private static void Grammar15(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("Propositional Calculus Grammar");
            grammar.BuildPattern("Start").NonTerminal("F").PatternEnd();
            grammar.BuildPattern("F").NonTerminal("C").Or.NonTerminal("S").Or.NonTerminal("P").Or.NonTerminal("U").PatternEnd();
            grammar.BuildPattern("C").NonTerminal("U").Terminal("⊃").NonTerminal("U").PatternEnd();
            grammar.BuildPattern("U").Terminal("(").NonTerminal("F").Terminal(")").Or.Terminal("~").NonTerminal("U").Or.NonTerminal("L").PatternEnd();
            grammar.BuildPattern("L").NonTerminal("L").Terminal("'").Or.Terminal("p").Or.Terminal("q").Or.Terminal("r").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("U").Terminal("∨").NonTerminal("S").Or.NonTerminal("U").Terminal("∨").NonTerminal("U").PatternEnd();
            grammar.BuildPattern("P").NonTerminal("U").Terminal("∧").NonTerminal("P").Or.NonTerminal("U").Terminal("∧").NonTerminal("U").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar16(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("GRE");
            grammar.BuildPattern("Start").NonTerminal("X").PatternEnd();
            grammar.BuildPattern("X").Terminal("a").Or.NonTerminal("X").Terminal("b").Or.NonTerminal("Y").Terminal("a").PatternEnd();
            grammar.BuildPattern("Y").Terminal("e").Or.NonTerminal("Y").Terminal("d").NonTerminal("Y").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar17(int stopAtStep)
        {
            /* An Efficient Context-Free Parsing Algorithm */
            RGG grammar = new RGG("NSE");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("B").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").Or.NonTerminal("S").NonTerminal("C").PatternEnd();
            grammar.BuildPattern("B").Terminal("b").Or.NonTerminal("D").NonTerminal("B").PatternEnd();
            grammar.BuildPattern("C").Terminal("c").PatternEnd();
            grammar.BuildPattern("D").Terminal("d").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar18(int stopAtStep)
        {
            /* An Improved Context-Free Recognizer */
            RGG grammar = new RGG("Example Empty Rules");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("B").NonTerminal("A").NonTerminal("C").PatternEnd();
            grammar.BuildPattern("A").TerminalEmpty.PatternEnd();
            grammar.BuildPattern("B").NonTerminal("C").NonTerminal("D").NonTerminal("C").PatternEnd();
            grammar.BuildPattern("C").TerminalEmpty.PatternEnd();
            grammar.BuildPattern("D").Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar19(int stopAtStep)
        {
            /* An Improved Context-Free Recognizer */
            RGG grammar = new RGG("Example Optional Rules");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("S").Or.Terminal("b").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").NonTerminal("A").Or.Terminal("b").NonTerminal("A").Or.TerminalEmpty.PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match aab
            // Test input: Non-Match 
        }

        private static void Grammar20(int stopAtStep)
        {
            /* An Improved Context-Free Recognizer */
            RGG grammar = new RGG("Example 1");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("S").Or.Terminal("a").Terminal("b").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").NonTerminal("A").Or.Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match aaab
            // Test input: Non-Match 
        }

        private static void Grammar21(int stopAtStep)
        {
            /* An Improved Context-Free Recognizer */
            RGG grammar = new RGG("Example 2");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("S").Terminal("a").Or.Terminal("a").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match aaa
            // Test input: Non-Match 
        }

        private static void Grammar22(int stopAtStep)
        {
            /* Leo */
            RGG grammar = new RGG("grammar");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").Terminal("b").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").NonTerminal("A").Or.Terminal("a").Terminal("a").NonTerminal("A").Or.TerminalEmpty.PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar23(int stopAtStep)
        {
            /* Leo */
            RGG grammar = new RGG("Example 2.2");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").Terminal("a").NonTerminal("S").Or.NonTerminal("C").PatternEnd();
            grammar.BuildPattern("C").Terminal("a").NonTerminal("C").Terminal("b").Or.TerminalEmpty.PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar24(int stopAtStep)
        {
            /* Leo */
            RGG grammar = new RGG("grammar (cubic size)");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").NonTerminal("B").PatternEnd();
            grammar.BuildPattern("A").Terminal("a").NonTerminal("A").Or.Terminal("a").PatternEnd();
            grammar.BuildPattern("B").Terminal("a").NonTerminal("B").Or.Terminal("b").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar25(int stopAtStep)
        {
            /* Leo */
            RGG grammar = new RGG("grammar (1)");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("A").Terminal("a").Or.NonTerminal("S").Terminal("a").Terminal("a").Or.TerminalEmpty.PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar26(int stopAtStep)
        {
            /* Leo */
            RGG grammar = new RGG("grammar (2)");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("X").Or.NonTerminal("Y").PatternEnd();
            grammar.BuildPattern("X").Terminal("a").NonTerminal("X").Terminal("b").Or.Terminal("a").Terminal("b").PatternEnd();
            grammar.BuildPattern("Y").Terminal("a").NonTerminal("Y").Terminal("b").Terminal("b").Or.TerminalEmpty.PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar27(int stopAtStep)
        {
            /* Leo */
            int i = 5;
            RGG grammar = new RGG("Leo grammar");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("R1").NonTerminal("S").Or.NonTerminal("R2").NonTerminal("S").Or.TerminalEmpty.PatternEnd();
            grammar.BuildPattern("R1").Terminal("x").NonTerminal("R1").Terminal("d").RepeatNext(i).Terminal("c").PatternEnd();
            grammar.BuildPattern("R2").Terminal("y").NonTerminal("R2").Terminal("d").Terminal("c").RepeatPrevious(i).PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }

        private static void Grammar28(int stopAtStep)
        {
            /* Horspool/McLean */
            RGG grammar = new RGG("Horspool grammar");
            grammar.BuildPattern("Start").NonTerminal("S").PatternEnd();
            grammar.BuildPattern("S").NonTerminal("E").PatternEnd();
            grammar.BuildPattern("E").NonTerminal("E").Terminal("+").NonTerminal("E").Or.Terminal("n").PatternEnd();
            grammar.BuildGraph(stopAtStep);
            grammar.PrintRGG(string.Format("Step {0}", stopAtStep), "Start");
            // Test input: Match original paper
            // Test input: Non-Match 
        }
    }
}
