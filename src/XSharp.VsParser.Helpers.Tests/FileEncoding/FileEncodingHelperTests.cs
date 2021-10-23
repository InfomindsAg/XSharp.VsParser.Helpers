using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestHelpers.TestHelperExtensions;
using XSharp.VsParser.Helpers.FileEncoding;
using System.IO;

namespace XSharp.Parser.Helpers.Tests.FileEncoding
{
    public class FileEncodingHelperTests
    {

        [Fact]
        public void ReadSourceCode()
        {
            var helper = new FileEncodingHelper();
            var programmEncoding = helper.DetectFileEncoding(CodeFile("program.prg"));
            var program = File.ReadAllText(CodeFile("program.prg"), programmEncoding.Encoding);

            var programmWin1252Encoding = helper.DetectFileEncoding(CodeFile("ProgramWin1252.prg"));
            var programWin1252 = File.ReadAllText(CodeFile("ProgramWin1252.prg"), programmWin1252Encoding.Encoding);

            program.Should().Be(programWin1252);
        }
    }
}