using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestFileName;

namespace XSharp.Parser.Helpers.Tests
{
    public class ParserHelperTests
    {

        [Fact]
        public void WithDefaultOptions()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("Program.prg"));

            result.Should().NotBeNull();
            result.OK.Should().BeTrue();
        }

    }
}
