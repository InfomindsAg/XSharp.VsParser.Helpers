using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class AssignmentExpressionContextTests : TestsFor<AssignmentExpressionContext>
    {
        [Fact]
        public void ReplacePropertyNameTest()
        {
            var code = WrapInMethod(@"self:Dummy := #aus");

            var expected = WrapInMethod(@"self:NewDummy := #aus");

            Rewrite(code, expected, r => r.ReplacePropertyName("NewDummy"));
        }

        [Fact]
        public void ReplaceValueTest()
        {
            var code = WrapInMethod(@"self:Dummy := #aus");

            var expected = WrapInMethod(@"self:Dummy := #ein");

            Rewrite(code, expected, r => r.ReplaceValue("#ein"));
        }

    }
}
