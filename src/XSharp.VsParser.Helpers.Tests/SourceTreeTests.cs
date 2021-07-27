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
        ParserHelper Parse()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("StringBuilderExamples.prg"));
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            return parser;
        }


        [Fact]
        public void SourceTreeEnumeratorTest()
        {
            Parse().SourceTree
                .WhereType<MethodContext>()
                .Select(q => q.ToValues().Name)
                .Should().BeEquivalentTo("Execute", "ConcatenateNoLineBreaks", "ConcatenateWithLineBreaks", "FluentApi", "FluentApiMultiLine", "Clear", "AppendFormat", "InsertAndRemove");
        }

        [Fact]
        public void WhereTypeWithPredicateTest()
        {
            Parse().SourceTree
                .WhereType<MethodContext>(q => q.ToValues().Name == "Execute")
                .Select(q => q.ToValues().Name)
                .Should().BeEquivalentTo("Execute");
        }


        [Fact]
        public void FirstParentOrDefaultTest()
        {
            Parse().SourceTree
                .WhereType<MethodContext>()
                .Select(q => q.FirstParentOrDefault<Class_Context>().ToValues()?.Name).Distinct()
                .Should().BeEquivalentTo("StringBuilderExamples");
        }

        [Fact]
        public void ToValueTests()
        {
            Parse().SourceTree
                .WhereType<MethodContext>()
                .Select(q => $"{q.FirstParentOrDefault<Class_Context>().ToValues().Name}.{q.ToValues().Name}")
                .Should().BeEquivalentTo(
                    "StringBuilderExamples.Execute",
                    "StringBuilderExamples.ConcatenateNoLineBreaks",
                    "StringBuilderExamples.ConcatenateWithLineBreaks",
                    "StringBuilderExamples.FluentApi",
                    "StringBuilderExamples.FluentApiMultiLine",
                    "StringBuilderExamples.Clear",
                    "StringBuilderExamples.AppendFormat",
                    "StringBuilderExamples.InsertAndRemove");
        }

        [Fact]
        public void AsEnumerableTest()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("StringBuilderExamples.prg"));

            var firstClass = parser.SourceTree.FirstOrDefaultType<Class_Context>();

            firstClass.AsEnumerable()
                .WhereType<MethodContext>()
                .Select(q => q.ToValues().Name)
                .Should().BeEquivalentTo(
                    "Execute",
                    "ConcatenateNoLineBreaks",
                    "ConcatenateWithLineBreaks",
                    "FluentApi",
                    "FluentApiMultiLine",
                    "Clear",
                    "AppendFormat",
                    "InsertAndRemove");
        }
    }
}
