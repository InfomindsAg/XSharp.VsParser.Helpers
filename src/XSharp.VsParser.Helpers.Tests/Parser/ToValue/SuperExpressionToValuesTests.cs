using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class SuperExpressionToValuesTests : TestsFor<SuperExpressionContext>
    {
        [Fact]
        public void NameTest()
        {
            var code = WrapInClass(@"method Dummy()
super:Dummy()
return nil");

            GetFirst(code).ToValues().MethodName.Should().Be("Dummy");
        }

        [Fact]
        public void DifferentNameTest()
        {
            var code = WrapInClass(@"method Dummy()
super:OtherDummy()
return nil");

            GetFirst(code).ToValues().MethodName.Should().Be("OtherDummy");
        }
    }
}
