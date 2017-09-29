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
            /* End Code */

            Console.WriteLine("End: {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            Console.WriteLine("Ended. Press Enter to continue");
            Console.Read();
        }
    }
}
