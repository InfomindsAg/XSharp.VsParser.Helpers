using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class LocalvarContextTests : TestsFor<LocalvarContext>
    {
        [Fact]
        public void ReplaceTypeEmptyTest()
        {
            var code = WrapInMethod(@"local a");

            var expected = WrapInMethod(@"local a as string");

            Rewrite(code, expected, r => r.ReplaceDataType("string"));
        }

        [Fact]
        public void ReplaceTypeDifferentTest()
        {
            var code = WrapInMethod(@"local a as int");

            var expected = WrapInMethod(@"local a as string");

            Rewrite(code, expected, r => r.ReplaceDataType("string"));
        }

        [Fact]
        public void ReplaceTypeMultipleEmptyTest()
        {
            var code = WrapInMethod(@"local a, b");

            var expected = WrapInMethod(@"local a as string, b as string");

            Rewrite(code, expected, r => r.ReplaceDataType("string"));
        }

        [Fact]
        public void ReplaceTypeMultipleEmptyWithInitTest()
        {
            var code = WrapInMethod(@"local a := """", b := """"");

            var expected = WrapInMethod(@"local a := """" as string, b := """" as string");

            Rewrite(code, expected, r => r.ReplaceDataType("string"));
        }


        

    }
}
