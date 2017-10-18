using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph
{
    public class RGGTransition
    {
        private TerminalPattern _terminalPattern;
        private string _nonTerminal;
        private List<string> _pushList;
        private List<string> _popList;

        public RGGNode From { get; private set; }
        public RGGNode To { get; private set; }
        public TransitionType TransitionType { get; private set; }

        public TerminalPattern Terminal
        {
            get
            {
                if (TransitionType == TransitionType.Push || TransitionType == TransitionType.Terminal)
                {
                    return _terminalPattern;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Push or Terminal: {0}", TransitionType));
                }
            }
            private set
            {
                if (TransitionType == TransitionType.Push || TransitionType == TransitionType.Terminal)
                {
                    _terminalPattern = value;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Push or Terminal: {0}", TransitionType));
                }
            }
        }

        public string NonTerminal
        {
            get
            {
                if (TransitionType == TransitionType.NonTerminalNonRecursive || TransitionType == TransitionType.NonTerminalRecursive)
                {
                    return _nonTerminal;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype NonTerminal: {0}", TransitionType));
                }
            }
            private set
            {
                if (TransitionType == TransitionType.NonTerminalNonRecursive || TransitionType == TransitionType.NonTerminalRecursive)
                {
                    _nonTerminal = value;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype NonTerminal: {0}", TransitionType));
                }
            }
        }

        public List<string> PushList
        {
            get
            {
                if (TransitionType == TransitionType.Push)
                {
                    return _pushList;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Push: {0}", TransitionType));
                }
            }
            private set
            {
                if (TransitionType == TransitionType.Push)
                {
                    _pushList = value;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Push: {0}", TransitionType));
                }
            }
        }

        public List<string> PopList
        {
            get
            {
                if (TransitionType == TransitionType.Pop)
                {
                    return _popList;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Pop: {0}", TransitionType));
                }
            }
            private set
            {
                if (TransitionType == TransitionType.Pop)
                {
                    _popList = value;
                }
                else
                {
                    throw new Exception(string.Format("Not expected transitiontype Pop: {0}", TransitionType));
                }
            }
        }

        public RGGTransition(RGGNode from, RGGNode to, TransitionType transitionType, params object[] args)
        {
            From = from;
            From.AddTransition(this);
            To = to;
            TransitionType = transitionType;
            switch (transitionType)
            {
                case TransitionType.PatternStart:
                case TransitionType.PatternEnd:
                    break;
                case TransitionType.NonTerminalNonRecursive:
                case TransitionType.NonTerminalRecursive:
                    if (args.Count() > 0 && args[0] is string)
                    {
                        NonTerminal = args[0] as string;
                    }
                    else
                    {
                        throw new Exception("NonTerminalNonRecursive/NonTerminalRecursive expect args of one string");
                    }
                    break;
                case TransitionType.Terminal:
                    if (args.Count() > 0 && args[0] is TerminalPattern)
                    {
                        Terminal = args[0] as TerminalPattern;
                    }
                    else
                    {
                        throw new Exception("Terminal expect args of one TerminalPattern");
                    }
                    break;
                case TransitionType.Pop:
                    if (args.Count() > 0)
                    {
                        for (int counter = 0; counter < args.Count(); counter += 1)
                        {
                            if (args[counter] is string)
                            {
                                PopList.Add(args[counter] as string);
                            }
                            else
                            {
                                throw new Exception(string.Format("{0} value of args expected to be string: was {1}", counter, args[counter].GetType().Name));
                            }
                        }
                    }
                    break;
                case TransitionType.Push:
                    if (args.Count() > 0)
                    {
                        if (args[0] is TerminalPattern)
                        {
                            Terminal = args[0] as TerminalPattern;
                        }
                        else
                        {
                            throw new Exception(string.Format("First value of args expected to be char: was {0}", args[0]));
                        }
                        for (int counter = 1; counter < args.Count(); counter += 1)
                        {
                            if (args[counter] is string)
                            {
                                PushList.Add(args[counter] as string);
                            }
                            else
                            {
                                throw new Exception(string.Format("{0} value of args expected to be string: was {1}", counter, args[counter].GetType().Name));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Push transition from node {0} to {1} missing arguments", From.Name, To.Name));
                    }
                    break;
            }
        }
    }
}
