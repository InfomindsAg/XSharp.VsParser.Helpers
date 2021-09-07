using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestHelpers.TestHelperExtensions;
using XSharp.Parser.Helpers.Tests.TestHelpers;

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

            parser.Comments.Should().BeEquivalentTo(
                new { Text = "///<summary>", StartLine = 1, StartColumn = 1, EndLine = 1, EndColumn = 12},
                new { Text = "///XMLHelp", StartLine = 2, StartColumn = 1, EndLine = 2, EndColumn = 10 },
                new { Text = "///</summary>", StartLine = 3, StartColumn = 1, EndLine = 3, EndColumn = 13 },
                new { Text = "// SingleLineComment", StartLine = 4, StartColumn = 29, EndLine = 4, EndColumn = 48 },
                new { Text = @"/*
  BlockComment
  */", StartLine = 5, StartColumn = 3, EndLine = 7, EndColumn = 4 });
        }

    }
}
