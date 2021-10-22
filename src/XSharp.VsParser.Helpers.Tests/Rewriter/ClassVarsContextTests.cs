using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class ClassVarsContextTests : TestsFor<ClassvarsContext>
    {

        [Fact]
        public void ReplaceTypeClassVarTest()
        {
            var code = WrapInClass(@"protected a");

            var expected = WrapInClass(@"protected a as string");

            Rewrite(code, expected, r => r.ReplaceType("string"));
        }

        [Fact]
        public void ReplaceTypeClassVarsTest()
        {
            // var code = WrapInClass(@"protected a, b");
            var code = WrapInClass(@"protected a, b as string");
            var expected = WrapInClass(@"protected a, b as string");

            Rewrite(code, expected, r => r.ReplaceType("string"));
        }

    }
}
