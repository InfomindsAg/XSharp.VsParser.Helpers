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

            Rewrite(code, expected, r => r.ReplaceAccessMemberMethodName("NewDummy"));
        }

        [Fact]
        public void ReplaceNameExpressionTest()
        {
            var code = WrapInMethod(@"Dummy()");

            var expected = WrapInMethod(@"NewDummy()");

            Rewrite(code, expected, r => r.ReplaceNameExpression("NewDummy"));
        }

        [Fact]
        public void DeleteAllArgumentsTest()
        {
            var code = WrapInMethod(@"self:Dummy(null_object)");

            var expected = WrapInMethod(@"self:Dummy()");

            Rewrite(code, expected, r => r.DeleteAllArguments());
        }

        [Fact]
        public void DeleteAllArgumentsNoArgumentsTest()
        {
            var code = WrapInMethod(@"self:Dummy()");

            var expected = WrapInMethod(@"self:Dummy()");

            Rewrite(code, expected, r => r.DeleteAllArguments());
        }

        [Fact]
        public void ReplaceAllArgumentsTest()
        {
            var code = WrapInMethod(@"self:Dummy(null_object)");

            var expected = WrapInMethod(@"self:Dummy(1, 2)");

            Rewrite(code, expected, r => r.ReplaceAllArguments("1, 2"));
        }

        [Fact]
        public void ReplaceAllArgumentsWhenEmptyTestTest()
        {
            var code = WrapInMethod(@"self:Dummy()");

            var expected = WrapInMethod(@"self:Dummy(1, 2)");

            Rewrite(code, expected, r => r.ReplaceAllArguments("1, 2"));
        }

    }
}
