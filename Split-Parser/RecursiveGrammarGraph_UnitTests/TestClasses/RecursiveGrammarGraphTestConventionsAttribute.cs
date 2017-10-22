using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph_UnitTests
{
    public class RecursiveGrammarGraphTestConventionsAttribute:AutoDataAttribute
    {
        public RecursiveGrammarGraphTestConventionsAttribute(): base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            this.Fixture.Customize(new OmitAutoPropertiesForTypesInNamespace("RecursiveGrammarGraph"));
        }
    }
}
