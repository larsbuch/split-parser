using RecursiveGrammarGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecursiveGrammarGraph_UnitTests
{
    public class RGG_UnitTests
    {
        [Theory, RecursiveGrammarGraphTestConventions]
        public void CreatePatternBuilder(string grammarName)
        {
            Exception expected = null;
            Exception actual = null;
            RGG rGG;
            try
            {
                rGG = new RGG(grammarName);
            }
            catch (Exception ex)
            {
                actual = ex;
            }
            Assert.Equal(expected, actual);
        }
    }
}
