using System;
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
        private Dictionary<string, Pattern> _buildPatternStart;

        public RGG(string grammarName, int stopBuildAtStep)
        {
            _rGGNodes = new Dictionary<string, RGGNode>();
            _patternStartRGGNodes = new Dictionary<string, RGGNode>();
            _grammarName = grammarName;
            _buildPatternStart = new Dictionary<string, Pattern>();
        }

        private Dictionary<string, Pattern> BuildPatternStart
        {
            get
            {
                return _buildPatternStart;
            }
        }

        public PatternPart BuildPattern(string patternName)
        {
            //Lookup Pattern and if not existing create one
            Pattern patternStart;
            if(!BuildPatternStart.TryGetValue(patternName, out patternStart))
            {
                patternStart = new Pattern(this, patternName);
                BuildPatternStart.Add(patternStart.Name, patternStart);
            }
            return patternStart.StartPattern();
        }

        public void BuildGraph()
        {
            foreach(Pattern pattern in BuildPatternStart.Values)
            {
                pattern.Build();
            }
        }

        private void BuildStepTwo()
        {
            foreach(RGGNode node in Nodes)
            {
                foreach(RGGTransition transition in node.Transitions)
                {
                    if(transition.TransitionType == TransitionType.NonTerminalNonRecursive)
                    {

                    }
                }
            }
            PrintRGG("Step 2", "Start");
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

        public void PrintRGG(string caption, string startNodeName)
        {
            string fileName = _grammarName + " - " + caption + ".dot";
            StringBuilder startBuilder = new StringBuilder();
            startBuilder.AppendFormat("digraph {0}", addQuotationMarks(_grammarName) + " {");
            startBuilder.AppendLine();
            startBuilder.AppendFormat("\t\tlabel = {0};", addQuotationMarks(_grammarName + " - " + caption));
            startBuilder.AppendLine();
            startBuilder.AppendLine("\t\trankdir=LR;");
            startBuilder.AppendFormat("\t\tnode [ shape = doublecircle, color = blue ]; \"{0}\" \"{0} End\";", startNodeName);
            startBuilder.AppendLine();
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
