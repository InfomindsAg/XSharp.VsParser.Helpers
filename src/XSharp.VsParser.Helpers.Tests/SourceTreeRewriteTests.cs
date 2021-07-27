using FluentAssertions;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using static XSharp.Parser.Helpers.Tests.TestFileName;

namespace XSharp.Parser.Helpers.Tests
{
    public class SourceTreeRewriteTests
    {
        [Fact]
        public void SourceTreeEnumeratorTest()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("StringBuilderExamples.prg"));
            result.Errors.Should().BeEmpty();

            foreach (var item in parser.SourceTree.WhereType<MethodContext>())
            {
                parser.SourceTree.Rewriter.ReplaceIdentifier(item.Sig.Id, item.ToValues().Name + "_XXX");
                parser.SourceTree.Rewriter.ReplaceCallingConvention(item.Sig, "clipper");
            }

            parser.ParseRewriter();
            foreach (var item in parser.SourceTree.WhereType<MethodContext>())
            {
                item.ToValues().Name.Should().EndWith("_XXX");
                item.Sig.CallingConvention?.GetText().Should().Be("clipper");
            }
        }

    }
}
