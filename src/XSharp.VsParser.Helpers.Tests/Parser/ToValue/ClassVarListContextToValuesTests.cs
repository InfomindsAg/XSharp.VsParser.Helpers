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
    public class ClassVarListContextToValuesTests : TestsFor<ClassvarContext>
    {
        [Fact]
        public void SingleTest()
        {
            var code = @"static global dummy := {} as usual";

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "dummy", InitExpression = "{}", Type = "usual" });
        }

        [Fact]
        public void MultipleTest()
        {
            var code = @"static global sa2ConvTabansi2html, saConvTabansi2html as array";

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "sa2ConvTabansi2html", InitExpression = (string)null, Type = "array" });
        }


        // 

    }
}
