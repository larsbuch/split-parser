using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class RGGNode
    {
        private Dictionary<string, RGGTransition> _pushTransitions;
        private Dictionary<string, RGGTransition> _popTransitions;
        private List<RGGTransition> _otherTransitions;

        public RGGNode(string name)
        {
            _pushTransitions = new Dictionary<string, RGGTransition>();
            _popTransitions = new Dictionary<string, RGGTransition>();
            _otherTransitions = new List<RGGTransition>();
            Name = name;
        }

        public string Name { get; private set; }

        public int TransitionCount
        {
            get
            {
                int counter = _pushTransitions.Count();
                counter += _popTransitions.Count();
                counter += _otherTransitions.Count();
                return counter;
            }
        }

        public IEnumerable<RGGTransition> Transitions
        {
            get
            {
                return _pushTransitions.Values.Concat(_popTransitions.Values).Concat(_otherTransitions);
            }
        }

        public void AddTransition(RGGTransition transition)
        {
            switch (transition.TransitionType)
            {
                case TransitionType.PatternStart:
                case TransitionType.PatternEnd:
                    _otherTransitions.Add(transition);
                    break;
                case TransitionType.Pop:
                    _popTransitions.Add(transition.To.Name, transition);
                    break;
                case TransitionType.Push:
                    _pushTransitions.Add(transition.Terminal.ToString(), transition);
                    break;
            }
        }

        public bool PushTransition(TerminalPattern inputChar, out RGGTransition transition)
        {
            RGGTransition tempTransition;
            if (_pushTransitions.TryGetValue(inputChar.ToString(), out tempTransition))
            {
                transition = tempTransition;
                return true;
            }
            else
            {
                transition = null;
                return false;
            }
        }

        public bool PopTransition(string stackState, out RGGTransition transition)
        {
            RGGTransition tempTransition;
            if (_popTransitions.TryGetValue(stackState, out tempTransition))
            {
                transition = tempTransition;
                return true;
            }
            else
            {
                transition = null;
                return false;
            }
        }

        public List<RGGTransition> NonPushTransitions
        {
            get
            {
                return _otherTransitions;
            }
        }
    }
}
