using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestHelpers.TestHelperExtensions;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using System.Text;

namespace XSharp.Parser.Helpers.Tests.Parser
{
    public class ParserHelperTests
    {

        [Fact]
        public void BuildWithDefaultOptionsTest()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("Program.prg"));

            result.Should().NotBeNull();
            result.OK.Should().BeTrue();
        }

        [Fact]
        public void CommentsTest()
        {
            var parser = @"///<summary>
///XMLHelp
///</summary>
class Test inherit BaseTest // SingleLineComment
  /*
  BlockComment
  */
end class".ParseText();

            var result = parser.Comments;
            result.Should().HaveCount(5);

            result[0].Should().BeEquivalentTo(new { Text = "///<summary>", StartLine = 1, StartColumn = 1, EndLine = 1, EndColumn = 12 });
            result[1].Should().BeEquivalentTo(new { Text = "///XMLHelp", StartLine = 2, StartColumn = 1, EndLine = 2, EndColumn = 10 });
            result[2].Should().BeEquivalentTo(new { Text = "///</summary>", StartLine = 3, StartColumn = 1, EndLine = 3, EndColumn = 13 });
            result[3].Should().BeEquivalentTo(new { Text = "// SingleLineComment", StartLine = 4, StartColumn = 29, EndLine = 4, EndColumn = 48 });
            result[4].Should().BeEquivalentTo(new { Text = @"/*
  BlockComment
  */", StartLine = 5, StartColumn = 3, EndLine = 7, EndColumn = 4 });
        }

        [Fact]
        public void ParallelTest()
        {
            var prjH = new XSharp.VsParser.Helpers.Project.ProjectHelper(CodeFile("XSharpExamples.xsproj"));
            var options = prjH.GetOptions();
            var file = prjH.GetSourceFiles(true).First();
            var sourceCode = new List<string>();
            var random = new Random();
            System.Threading.Tasks.Parallel.ForEach(Enumerable.Range(1, 2000), item =>
            {
                var sb = new StringBuilder();
                sb.Clear().AppendLine("Function Dummy() as int").AppendLine("var i := 0");
                for (int i = 1; i < random.Next(10, 50); i++)
                    sb.AppendLine("i := i + " + i.ToString());
                sb.AppendLine("return i");
                var parser = ParserHelper.BuildWithOptionsList(options);
                string.Join(", ", parser.ParseText(sb.ToString(), $"Dummy{item}.prg").Errors).Should().BeEmpty();
            });
        }

        [Fact]
        public void LinesTest()
        {
            var lines = new List<string> { "class dummy", "method test()", "return nil", "end class" };
            var code = string.Join(Environment.NewLine, lines);

            code.ParseText().SourceCodeLines.Should().BeEquivalentTo(lines);

            code = string.Join("\n", lines);

            code.ParseText().SourceCodeLines.Should().BeEquivalentTo(lines);
        }


        [Fact]
        public void EmptyRewriteNoChangesTest()
        {
            var code = @"///<summary>
///XMLHelp
///</summary>
class Test inherit BaseTest // SingleLineComment
  /*
  BlockComment
  */
end class";
            var parser = code.ParseText();

            parser.Tree.Rewriter.GetText().Should().Be(code);
        }

        [Fact]
        public void EmptyRewriteChangesException1Test()
        {
            var parser = @"class Test
method Dummy() as string
  return """""""" // this breaks the rewrite

end class".ParseText();
            Action emptyRewrite = () => parser.Tree.Rewriter.GetText();

            emptyRewrite.Should().Throw<RewriterException>();
        }

        [Fact]
        public void EmptyRewriteChangesException2Test()
        {
            var parser = @"class Test
method Dummy() as string
  return '''' // this breaks the rewrite

end class".ParseText();
            Action emptyRewrite = () => parser.Tree.Rewriter.GetText();

            emptyRewrite.Should().Throw<RewriterException>();
        }


        [Fact]
        public void DummyTestForSpecificFile()
        {
            var fileName = @"";
            if (!string.IsNullOrEmpty(fileName))
            {
                var parser = fileName.ParseFile();
                parser.Tokens.Count.Should().BeGreaterOrEqualTo(0);
                _ = parser.Tree.Rewriter;
            }
        }
    }
}