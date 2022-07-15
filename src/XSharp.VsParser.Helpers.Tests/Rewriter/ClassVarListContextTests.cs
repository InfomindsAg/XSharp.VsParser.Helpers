using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    /*
    public class ClassVarListContextTests : TestsFor<ClassVarListContext>
    {

        [Fact]
        public void ReplaceTypeClassVarListTest()
        {
            var code = @"static global dummy := {}";

            var expected = @"static global dummy := {} as usual";

            Rewrite(code, expected, r => r.ReplaceType("usual"));
        }

        [Fact]
        public void ReplaceTypeClassVarsTest()
        {
            // var code = WrapInClass(@"protected a, b");
            var code = @"static global dummy1 := {}, dummy2 := {}";
            var expected = @"static global dummy1 := {}, dummy2 := {} as usual";

            Rewrite(code, expected, r => r.ReplaceType("usual"));
        }

    }*/
}
