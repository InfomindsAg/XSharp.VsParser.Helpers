using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class PropertyContextTests : TestsFor<PropertyContext>
    {
        [Fact]
        public void ReplaceTypeEmptyGetInInterfaceTest()
        {
            var code = WrapInInterface(@"property Dummy get");

            var expected = WrapInInterface(@"property Dummy as string get");

            Rewrite(code, expected, r => r.ReplaceType("string"));
        }

        [Fact]
        public void ReplaceTypeEmptyGetSetInClassTest()
        {
            var code = WrapInClass(@"property Dummy get set");

            var expected = WrapInClass(@"property Dummy as string get set");

            Rewrite(code, expected, r => r.ReplaceType("string"));
        }


    }
}
