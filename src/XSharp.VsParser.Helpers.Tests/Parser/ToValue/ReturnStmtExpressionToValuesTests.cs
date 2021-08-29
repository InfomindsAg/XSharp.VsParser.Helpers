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
    public class ReturnStmtExpressionToValuesTests : TestsFor<ReturnStmtContext>
    {
        [Fact]
        public void NoExpressionTest()
        {
            var code = WrapInClass(@"method Dummy()
return");

            GetFirst(code).ToValues().ExpressionText.Should().BeNullOrEmpty();
        }

        [Fact]
        public void BoolTest()
        {
            var code = WrapInClass(@"method Dummy()
return false");

            GetFirst(code).ToValues().ExpressionText.Should().Be("false");
        }
    }
}
