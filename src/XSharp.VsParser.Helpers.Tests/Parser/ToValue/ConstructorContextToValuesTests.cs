using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class ConstructorContextToValuesTests : TestsFor<ConstructorContext>
    {
        [Fact]
        public void NoConventionTest()
        {
            var code = WrapInClass(@"constructor()
return nil");

            GetFirst(code).ToValues().CallingConvention.Should().BeNullOrEmpty();
        }

        [Fact]
        public void StrictTest()
        {
            var code = WrapInClass(@"constructor() strict
return nil");

            GetFirst(code).ToValues().CallingConvention.Should().Be("strict");
        }
    }
}
