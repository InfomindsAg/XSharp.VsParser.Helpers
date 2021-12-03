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
    public class ConstructorchainToValuesTests : TestsFor<ConstructorchainContext>
    {
        [Fact]
        public void ArgumentsTest()
        {
            var code = WrapInClass(@"constructor(dummy1, dummy2)
super(dummy1, dummy2)
return");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                Arguments = new[] { new { Value = "dummy1" }, new { Value = "dummy2" } }
            });
        }
    }
}
