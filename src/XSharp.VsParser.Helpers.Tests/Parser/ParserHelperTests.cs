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
            var parser = @"
///<summary>
///XMLHelp
///</summary>
class Test inherit BaseTest // SingleLineComment
/*
BlockComment
*/
end class".ParseText();

            parser.Comments.Select(q => q.Text).Should().BeEquivalentTo(
                "///<summary>",
                "///XMLHelp",
                "///</summary>",
                "// SingleLineComment",
                @"/*
BlockComment
*/");
        }

    }
}
