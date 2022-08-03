using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Parser.Values;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class ClassvarsContextToValuesTests : TestsFor<ClassvarsContext>
    {
        [Fact]
        public void SingleTest()
        {
            var code = WrapInClass(@"private static dummy := {} as usual");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                Modifiers = new string[] { "private", "static" },
                Vars = new object[]
                {
                    new { Name = "dummy", InitExpression = "{}", Type = "usual" },
                }
            });
        }

        [Fact]
        public void MultipleTest()
        {
            var code = WrapInClass(@"protected a := 2, b as int, c as string");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                Modifiers = new string[] { "protected" },
                Vars = new object[]
                {
                    new { Name = "a", InitExpression = "2", Type = "int" },
                    new { Name = "b", InitExpression = (string)null, Type = "int" },
                    new { Name = "c", InitExpression = (string)null, Type = "string" }
                }
            });
        }

    }
}
