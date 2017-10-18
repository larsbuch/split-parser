using RecursiveGrammarGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognizer
{
    public class Recognizer
    {
        private RGG _RecursiveGrammarGraph;

        public Recognizer(RGG rGG)
        {
            _RecursiveGrammarGraph = rGG;
        }

        private RGG RecursiveGrammarGraph
        {
            get
            {
                return _RecursiveGrammarGraph;
            }
        }

        public bool Recognize(string inputToRecognizeOrReject)
        {
            List<TerminalPattern> terminalPatterns = new List<TerminalPattern>();
            foreach(char input in inputToRecognizeOrReject.ToCharArray())
            {
                terminalPatterns.Add(new TerminalPattern(input.ToString()));
            }
            bool recognized = false;
            bool consumeCharacter = false;
            RGGNode currentNode = RecursiveGrammarGraph.GetNode("Start"); //To Recognition path: one for each pattern start from start
            RGGTransition tempTransition;
            Stack<RGGNode> nodeStack = new Stack<RGGNode>();//To Recognition path. Later replace with GSS
            foreach (TerminalPattern terminalPattern in terminalPatterns)
            {
                consumeCharacter = false;
                while (!consumeCharacter)
                {
                    if (currentNode.PushTransition(terminalPattern, out tempTransition))
                    {
                        // TODO push to stack
                        currentNode = tempTransition.To;
                        consumeCharacter = true;
                    }
                    else
                    {
                        if ((nodeStack.Count() > 0) && (currentNode.PopTransition(nodeStack.Peek().Name, out tempTransition)))
                        {
                            nodeStack.Pop();
                            currentNode = tempTransition.To;
                        }
                        else
                        {
                            foreach (RGGTransition transition in currentNode.NonPushTransitions)
                            {
                                switch (transition.TransitionType)
                                {
                                    case TransitionType.PatternStart:
                                        currentNode = transition.To;
                                        break;
                                    case TransitionType.PatternEnd:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            // TODO check that stack is empty and last transition is PatternEnd for start
            return recognized;
        }
    }
}
