using FluentAssertions;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Parser.Values;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class ClassVarContextTests : TestsFor<ClassvarContext>
    {

        [Fact]
        public void ReplaceTypeClassVarTest()
        {
            var code = WrapInClass(@"protected a");

            var expected = WrapInClass(@"protected a as string");

            Rewrite(code, expected, r => r.ReplaceType("string"));
        }

        [Fact]
        public void ReplaceTypeClassVarsTest()
        {
            var code = WrapInClass(@"protected a as int, b, c as usual");
            var expected = WrapInClass(@"protected a as int, b as string, c as usual");
            var parser = code.ParseText();
            var vars = GetVars(parser);

            var toBeReplacedVar = vars.Single(x => x.Name.Equals("b")).Context;

            parser.Tree.RewriterFor(toBeReplacedVar).ReplaceType("string");
            AssertRewriterTextEquals(parser, expected);
        }

        [Fact]
        public void ReplaceTypeClassVarsTest2()
        {
            var code = WrapInClass(@"protected a as int, b, c as usual");
            var expected = WrapInClass(@"protected a as int, b as usual, c as string");
            var parser = code.ParseText();
            var vars = GetVars(parser);

            var toBeReplacedVar = vars.Single(x => x.Name.Equals("c")).Context;

            parser.Tree.RewriterFor(toBeReplacedVar).ReplaceType("string");
            AssertRewriterTextEquals(parser, expected);
        }

        [Fact]
        public void ReplaceTypeClassVarsTest3()
        {
            var code = WrapInClass(@"protected a, b, c as usual");
            var expected = WrapInClass(@"protected a as usual, b as string, c as usual");
            var parser = code.ParseText();
            var vars = GetVars(parser);

            var toBeReplacedVar = vars.Single(x => x.Name.Equals("b")).Context;

            parser.Tree.RewriterFor(toBeReplacedVar).ReplaceType("string");
            AssertRewriterTextEquals(parser, expected);
        }

        private static ClassvarContextValues[] GetVars(ParserHelper parser)
        {
            return parser.Tree.WhereType<ClassvarsContext>()
                            .Single()
                            .ToValues().Vars;
        }

        private void AssertRewriterTextEquals(ParserHelper parser, string expected)
        {
            var resultLines = parser.Tree.Rewriter.GetText().SplitLines();
            var expectedLines = expected.SplitLines();
            for (var i = 0; i < resultLines.Length; i++)
                resultLines[i].Should().Be(expectedLines.ElementAtOrDefault(i), $"Line {i}");
        }
    }
}
