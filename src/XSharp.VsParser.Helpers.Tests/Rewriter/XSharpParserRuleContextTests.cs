using FluentAssertions;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class XSharpParserRuleContextTests : TestsFor<XSharpParserRuleContext>
    {
        [Fact]
        public void ReplaceTest()
        {
            var code = WrapInMethod(@"self:Icon := oIcon");

            var expected = WrapInMethod(@"self:Icon := (Icon)oIcon");

            var parser = code.ParseText();
            var assignment = parser.Tree.FirstOrDefaultType<AssignmentExpressionContext>();
            parser.Tree.RewriterFor(assignment.Right).Replace("(Icon)oIcon");

            parser.Tree.Rewriter.GetText().Should().Be(expected);
        }
    }
}
