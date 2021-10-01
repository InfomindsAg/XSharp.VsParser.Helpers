using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class MethodCallContextTests : TestsFor<MethodCallContext>
    {
        [Fact]
        public void ReplaceMethodNameTest()
        {
            var code = WrapInMethod(@"self:Dummy()");

            var expected = WrapInMethod(@"self:NewDummy()");

            Rewrite(code, expected, r => r.ReplaceMethodName("NewDummy"));
        }

        [Fact]
        public void DeleteAllParametersTest()
        {
            var code = WrapInMethod(@"self:Dummy(null_object)");

            var expected = WrapInMethod(@"self:Dummy()");

            Rewrite(code, expected, r => r.DeleteAllArguments());
        }


    }
}
