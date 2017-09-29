﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class RGG
    {
        private Dictionary<string, RGGNode> _rGGNodes;
        private Dictionary<string, RGGNode> _patternStartRGGNodes;
        private string _grammarName;

        public RGG(string grammarName, int stopBuildAtStep)
        {
            _rGGNodes = new Dictionary<string, RGGNode>();
            _patternStartRGGNodes = new Dictionary<string, RGGNode>();
            _grammarName = grammarName;
            BuildStepOne();
            if (stopBuildAtStep > 1)
            {
                BuildStepTwo();
                if (stopBuildAtStep > 2)
                {
                    BuildStepThree();
                    if (stopBuildAtStep > 3)
                    {
                        BuildStepFour();
                        if (stopBuildAtStep > 4)
                        {
                            BuildStepFive();
                        }
                    }
                }
            }
        }

        /* Wireup patterns*/
        private void BuildStepOne()
        {
            CreateRGGTransition("Start", "S1", TransitionType.PatternStart);
            CreateRGGTransition("S1", "S2", TransitionType.NonTerminalNonRecursive, "E");
            CreateRGGTransition("S2", "End", TransitionType.PatternEnd);
            CreateRGGTransition("E", "E1", TransitionType.PatternStart);
            CreateRGGTransition("E1", "E2", TransitionType.NonTerminalRecursive, "E");
            CreateRGGTransition("E2", "E3", TransitionType.NonTerminalNonRecursive, "Q");
            CreateRGGTransition("E3", "E4", TransitionType.NonTerminalNonRecursive, "F");
            CreateRGGTransition("E4", "E End", TransitionType.PatternEnd);
            CreateRGGTransition("E", "E5", TransitionType.PatternStart);
            CreateRGGTransition("E5", "E6", TransitionType.NonTerminalNonRecursive, "F");
            CreateRGGTransition("E6", "E End", TransitionType.PatternEnd);
            CreateRGGTransition("Q", "Q1", TransitionType.PatternStart);
            CreateRGGTransition("Q1", "Q2", TransitionType.Terminal, '+');
            CreateRGGTransition("Q2", "Q End", TransitionType.PatternEnd);
            CreateRGGTransition("Q", "Q3", TransitionType.PatternStart);
            CreateRGGTransition("Q3", "Q4", TransitionType.Terminal, '-');
            CreateRGGTransition("Q4", "Q End", TransitionType.PatternEnd);
            CreateRGGTransition("F", "F1", TransitionType.PatternStart);
            CreateRGGTransition("F1", "F2", TransitionType.Terminal, 'a');
            CreateRGGTransition("F2", "F End", TransitionType.PatternEnd);
            PrintRGG("Step 1");
        }

        private void BuildStepTwo()
        {
            throw new NotImplementedException();
        }

        private void BuildStepThree()
        {
            throw new NotImplementedException();
        }

        private void BuildStepFour()
        {
            throw new NotImplementedException();
        }

        private void BuildStepFive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RGGNode> Nodes
        {
            get
            {
                return _rGGNodes.Values;
            }
        }

        public void CreateRGGTransition(string from, string to, TransitionType transitionType, params object[] args)
        {
            RGGNode fromNode = GetNode(from);
            if (transitionType == TransitionType.PatternStart)
            {
                if (!_patternStartRGGNodes.ContainsKey(from))
                {
                    _patternStartRGGNodes.Add(from, fromNode);
                }
            }
            RGGTransition transition = new RGGTransition(fromNode, GetNode(to), transitionType, args);
        }

        public RGGNode GetPatternStart(string patternName)
        {
            RGGNode returnNode;
            if (_patternStartRGGNodes.TryGetValue(patternName, out returnNode))
            {
                return returnNode;
            }
            else
            {
                throw new Exception(string.Format("Unknown pattern name {0}", patternName));
            }
        }

        public RGGNode GetNode(string nodeName)
        {
            RGGNode returnNode;
            if (_rGGNodes.TryGetValue(nodeName, out returnNode))
            {
                return returnNode;
            }
            else
            {
                returnNode = new RGGNode(nodeName);
                _rGGNodes.Add(returnNode.Name, returnNode);
                return returnNode;
            }
        }

        #region DOT export

        public void PrintRGG(string caption)
        {
            string fileName = _grammarName + " - " + caption + ".dot";
            StringBuilder startBuilder = new StringBuilder();
            startBuilder.AppendFormat("digraph {0}", addQuotationMarks(_grammarName) + " {");
            startBuilder.AppendLine();
            startBuilder.AppendFormat("\t\tlabel = {0};", addQuotationMarks(_grammarName + " - " + caption));
            startBuilder.AppendLine();
            startBuilder.AppendLine("\t\trankdir=LR;");
            startBuilder.AppendLine("\t\tnode [ shape = doublecircle, color = blue ]; \"Start\" \"End\";");
            List<string> patternNodes = new List<string>();
            StringBuilder transitionBuilder = new StringBuilder();
            transitionBuilder.AppendLine("\t\tnode [ shape = circle, color = black ];");
            foreach (RGGNode node in Nodes)
            {
                /* Ignore end nodes */
                if (node.TransitionCount > 0)
                {
                    foreach (RGGTransition transition in node.Transitions)
                    {
                        transitionBuilder.AppendFormat("\t{0} -> {1} [ label = ", addQuotationMarks(transition.From.Name), addQuotationMarks(transition.To.Name));
                        switch (transition.TransitionType)
                        {
                            case TransitionType.PatternStart:
                                transitionBuilder.Append(addQuotationMarks("<Pattern Start>"));
                                transitionBuilder.Append(", color = blue");
                                if (!transition.From.Name.Equals("Start"))
                                {
                                    patternNodes.Add(addQuotationMarks(transition.From.Name));
                                }
                                break;
                            case TransitionType.PatternEnd:
                                transitionBuilder.Append(addQuotationMarks("<Pattern End>"));
                                transitionBuilder.Append(", color = blue");
                                if (!transition.To.Name.Equals("End"))
                                {
                                    patternNodes.Add(addQuotationMarks(transition.To.Name));
                                }
                                break;
                            case TransitionType.NonTerminalRecursive:
                            case TransitionType.NonTerminalNonRecursive:
                                transitionBuilder.AppendFormat(addQuotationMarks("<{0}>"), transition.NonTerminal);
                                break;
                            case TransitionType.Terminal:
                                transitionBuilder.Append(addQuotationMarks(transition.Terminal));
                                break;
                        }
                        transitionBuilder.AppendLine(" ];");
                    }
                }
            }
            startBuilder.AppendFormat("\t\tnode [ shape = circle, color = blue ];{0};", string.Join(" ", patternNodes.Distinct()));
            startBuilder.AppendLine();
            transitionBuilder.AppendLine("}");
            File.WriteAllText(fileName, startBuilder.ToString() + transitionBuilder.ToString());
        }

        private string addQuotationMarks(char character)
        {
            return addQuotationMarks(string.Format("{0}", character));
        }

        private string addQuotationMarks(string nodeName)
        {
            return "\"" + nodeName + "\"";
        }

        #endregion
    }
}
