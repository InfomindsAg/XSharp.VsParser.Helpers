using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class ClassvarsContextToValuesTests : TestsFor<ClassvarsContext>
    {
        [Fact]
        public void SingleTest()
        {
            var code = WrapInClass(@"protected a as string");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Modifiers = new string[] { "protected" }, Names = new string[] { "a" }, Type = "string" });
        }

        [Fact]
        public void MultipleTest()
        {
            var code = WrapInClass(@"protected a, b, c as string");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Modifiers = new string[] { "protected" }, Names = new string[] { "a", "b", "c" }, Type = "string" });
        }

    }
}
