using RecursiveGrammarGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecursiveGrammarGraph_UnitTests
{
    public class PatternBuilder_UnitTests
    {
        [Theory, RecursiveGrammarGraphTestConventions]
        public void CreatePatternBuilder(string grammarName, string patternName)
        {
            Exception expected = null;
            Exception actual = null;
            RGG rGG = new RGG(grammarName);
            IStartPatternPart patternPart;
            try
            {
                patternPart = rGG.BuildPattern(patternName);
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void PartialPatternBuilder_Terminal(string grammarName, string patternName, string terminalName)
        {
            Exception expected = null;
            Exception actual = null;
            RGG rGG = new RGG(grammarName);
            IPatternPart patternPart;
            try
            {
                patternPart = rGG.BuildPattern(patternName).Terminal(terminalName);
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void EndPatternBuilder(string grammarName, string patternName, string terminalName)
        {
            Exception expected = null;
            Exception actual = null;
            RGG rGG = new RGG(grammarName);
            try
            {
                rGG.BuildPattern(patternName).Terminal(terminalName).PatternEnd();
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).Terminal(terminalName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_TerminalEmpty(string grammarName, string patternName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).TerminalEmpty.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(Constants.Empty, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_NonTerminalNonRecursive(string grammarName, string patternName, string nonTerminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).NonTerminal(nonTerminalName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Non Terminal Non Recursive
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.NonTerminalNonRecursive, candidateTransition.TransitionType);
            Assert.Equal(nonTerminalName, candidateTransition.NonTerminal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_NonTerminalRecursive(string grammarName, string patternName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).NonTerminal(patternName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Non Terminal Recursive
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.NonTerminalRecursive, candidateTransition.TransitionType);
            Assert.Equal(patternName, candidateTransition.NonTerminal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_Or(string grammarName, string patternName, string terminalName1, string terminalName2)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).Terminal(terminalName1).Or.Terminal(terminalName2).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(2, startPatternNode.TransitionCount);
            int counter = 0;
            foreach (RGGTransition startTransition in startPatternNode.Transitions)
            {
                // Pattern Start
                RGGTransition candidateTransition = startTransition;
                Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
                RGGNode candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(1, candidateNode.TransitionCount);
                // Terminal
                candidateTransition = candidateNode.Transitions.First();
                Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
                Assert.Equal(counter == 0?terminalName1:terminalName2, candidateTransition.Terminal.ToString());
                candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(1, candidateNode.TransitionCount);
                // Pattern End
                candidateTransition = candidateNode.Transitions.First();
                Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
                candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(0, candidateNode.TransitionCount);

                counter += 1;
            }
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_GroupStart_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).GroupStart.Terminal(terminalName).GroupEnd.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(Constants.UnnamedGroupStart, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, Constants.UnnamedGroupStart), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_NamedGroupStart_Terminal(string grammarName, string patternName, string groupName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).NamedGroupStart(groupName).Terminal(terminalName).GroupEnd.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // NamedGroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(groupName, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, groupName), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_NamedGroupStart_Or(string grammarName, string patternName, string groupName, string terminalName1, string terminalName2)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).NamedGroupStart(groupName).Terminal(terminalName1).Or.Terminal(terminalName2).GroupEnd.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // NamedGroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(groupName, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(2, candidateNode.TransitionCount);
            int counter = 0;
            string nextName = string.Empty;
            foreach (RGGTransition groupStartTransition in candidateNode.Transitions)
            {
                // Terminal
                candidateTransition = groupStartTransition;
                Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
                Assert.Equal(counter == 0 ? terminalName1 : terminalName2, candidateTransition.Terminal.ToString());
                candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(1, candidateNode.TransitionCount);
                if (nextName == string.Empty)
                {
                    nextName = candidateNode.Name;
                }
                else
                {
                    Assert.Equal(nextName, candidateNode.Name);
                }

                counter += 1;
            }
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, groupName), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_NamedGroupStart_Or2(string grammarName, string patternName, string groupName, string terminalName1, string terminalName2, string terminalName3, string terminalName4)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).NamedGroupStart(groupName).Terminal(terminalName1).Terminal(terminalName2).Or.Terminal(terminalName3).Terminal(terminalName4).GroupEnd.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // NamedGroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(groupName, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(2, candidateNode.TransitionCount);
            int counter = 0;
            string nextName = string.Empty;
            foreach (RGGTransition groupStartTransition in candidateNode.Transitions)
            {
                // Terminal
                candidateTransition = groupStartTransition;
                Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
                Assert.Equal(counter == 0 ? terminalName1 : terminalName3, candidateTransition.Terminal.ToString());
                candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(1, candidateNode.TransitionCount);
                // Terminal
                candidateTransition = candidateNode.Transitions.First(); ;
                Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
                Assert.Equal(counter == 0 ? terminalName2 : terminalName4, candidateTransition.Terminal.ToString());
                candidateNode = candidateTransition.To;
                Assert.True(candidateNode.Name.StartsWith(patternName));
                Assert.Equal(1, candidateNode.TransitionCount);
                if (nextName == string.Empty)
                {
                    nextName = candidateNode.Name;
                }
                else
                {
                    Assert.Equal(nextName, candidateNode.Name);
                }

                counter += 1;
            }
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, groupName), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatNext_Precise_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).RepeatNext(5).Terminal(terminalName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatNext_Range_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).RepeatNext(2, 5).Terminal(terminalName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatNext_Optional_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).OptionalNext.Terminal(terminalName).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatNext_GroupStart_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).RepeatNext(0,3).GroupStart.Terminal(terminalName).GroupEnd.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(Constants.UnnamedGroupStart, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, Constants.UnnamedGroupStart), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatPrevious_Precise_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).Terminal(terminalName).RepeatPrevious(5).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatPrevious_Range_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).Terminal(terminalName).RepeatPrevious(2, 5).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatPrevious_Optional_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).Terminal(terminalName).OptionalPrevious.PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

        [Theory, RecursiveGrammarGraphTestConventions]
        public void FullPattern_RepeatPrevious_GroupStart_Terminal(string grammarName, string patternName, string terminalName)
        {
            RGG rGG = new RGG(grammarName);
            rGG.BuildPattern(patternName).GroupStart.Terminal(terminalName).GroupEnd.RepeatPrevious(0,3).PatternEnd();
            rGG.BuildGraph(1);
            RGGNode startPatternNode = rGG.GetPatternStart(patternName);
            Assert.Equal(1, startPatternNode.TransitionCount);
            // Pattern Start
            RGGTransition candidateTransition = startPatternNode.Transitions.First();
            Assert.Equal(TransitionType.PatternStart, candidateTransition.TransitionType);
            RGGNode candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupStart
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupStart, candidateTransition.TransitionType);
            Assert.Equal(Constants.UnnamedGroupStart, candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Terminal
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.Terminal, candidateTransition.TransitionType);
            Assert.Equal(terminalName, candidateTransition.Terminal.ToString());
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // GroupEnd
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.GroupEnd, candidateTransition.TransitionType);
            Assert.Equal(string.Format(Constants.EndPattern, Constants.UnnamedGroupStart), candidateTransition.Internal);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(1, candidateNode.TransitionCount);
            // Pattern End
            candidateTransition = candidateNode.Transitions.First();
            Assert.Equal(TransitionType.PatternEnd, candidateTransition.TransitionType);
            candidateNode = candidateTransition.To;
            Assert.True(candidateNode.Name.StartsWith(patternName));
            Assert.Equal(0, candidateNode.TransitionCount);
        }

    }
}
