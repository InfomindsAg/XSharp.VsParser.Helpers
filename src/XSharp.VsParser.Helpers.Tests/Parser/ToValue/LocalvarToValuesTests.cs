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
    public class LocalvarToValuesTests : TestsFor<LocalvarContext>
    {
        [Fact]
        public void NameTest()
        {
            var code = WrapInMethod(@"local dummy");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "dummy", Type = (string)null });
        }

        [Fact]
        public void TypeWithInitTest()
        {
            var code = WrapInMethod(@"local dummy := 1 as int");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "dummy", Type = "int", InitExpression = "1" });
        }



    }
}
