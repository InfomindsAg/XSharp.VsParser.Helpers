using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XSharp.Parser.Helpers;
using static IM.DevTools.XsFormToWinForm.Parser.Tests.TestFileName;

namespace IM.DevTools.XsFormToWinForm.Parser.Tests
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
