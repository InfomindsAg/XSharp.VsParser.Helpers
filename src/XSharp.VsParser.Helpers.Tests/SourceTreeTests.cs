using FluentAssertions;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using static XSharp.Parser.Helpers.Tests.TestFileName;

namespace XSharp.Parser.Helpers.Tests
{
    public class SourceTreeTests
    {
        [Fact]
        public void SourceTreeEnumeratorTest()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("StringBuilderExamples.prg"));

            parser.SourceTree
                .Where(q => q is MethodContext)
                .Select(q => ((MethodContext)q).Sig.identifier().GetText())
                .Should().BeEquivalentTo("Execute", "ConcatenateNoLineBreaks", "ConcatenateWithLineBreaks", "FluentApi", "FluentApiMultiLine", "Clear", "AppendFormat", "InsertAndRemove");
        }
    }
}
