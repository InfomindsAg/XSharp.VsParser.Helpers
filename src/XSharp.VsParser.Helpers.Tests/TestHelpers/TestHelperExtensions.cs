using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;

namespace XSharp.Parser.Helpers.Tests.TestHelpers
{
    static class TestHelperExtensions
    {
        public static string UnitTestData(string fileName)
            => Path.Combine("_UnitTestData", fileName);

        public static string CodeFile(string fileName)
            => UnitTestData(Path.Combine("CodeFiles", fileName));

        public static string ProjectFile(string fileName)
            => UnitTestData(Path.Combine("ProjectFiles", fileName));


        public static ParserHelper ParseText(this string code, string fileName = "dummy.prg")
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseText(code, fileName);
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            return parser;
        }

        public static ParserHelper ParseFile(this string fileName)
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(fileName);
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            return parser;
        }

        public static string[] SplitLines(this string code)
            => code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(q => q.TrimEnd()).ToArray();
    }
}
