using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecursiveGrammarGraph_UnitTests
{
    public class OmitPropertyForTypeInNamespace : ISpecimenBuilder
    {
        private readonly string ns;

        public OmitPropertyForTypeInNamespace(string ns)
        {
            this.ns = ns;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (IsProperty(request) &&
                IsDeclaringTypeInNamespace((PropertyInfo)request))
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen();
        }

        private bool IsProperty(object request)
        {
            return request is PropertyInfo;
        }

        private bool IsDeclaringTypeInNamespace(PropertyInfo property)
        {
            var declaringType = property.DeclaringType;
            return declaringType.Namespace.Equals(
                this.ns,
                StringComparison.OrdinalIgnoreCase);
        }
    }

    public class OmitAutoPropertiesForTypesInNamespace : ICustomization
    {
        private readonly string ns;

        public OmitAutoPropertiesForTypesInNamespace(string ns)
        {
            this.ns = ns;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new OmitPropertyForTypeInNamespace(this.ns));
        }
    }
}
