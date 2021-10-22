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
    public class PropertyToValuesTests : TestsFor<PropertyContext>
    {
        [Fact]
        public void NameTest()
        {
            var code = WrapInClass(@"property dummy get set");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "dummy", Type = (string)null });
        }

        [Fact]
        public void TypeWithInitTest()
        {
            var code = WrapInClass(@"property dummy as int get set");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "dummy", Type = "int" });
        }



    }
}
