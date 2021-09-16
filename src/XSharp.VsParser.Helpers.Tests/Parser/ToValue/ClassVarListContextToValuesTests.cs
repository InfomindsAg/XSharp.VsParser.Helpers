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
    public class ClassVarListContextToValuesTests : TestsFor<ClassVarListContext>
    {
        [Fact]
        public void SingleTest()
        {
            var code = @"static global dummy := {} as usual";

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Variables = new[] { new { Name = "dummy", InitExpression = "{}" } }, Type = "usual" });
        }

    }
}
