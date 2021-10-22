using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class FuncprocContextTests : TestsFor<FuncprocContext>
    {

        [Fact]
        public void ReplaceNoReturnTypeWithVoidTest()
        {
            var code = @"function Dummy()
return nil";

            var expected = @"function Dummy() as void
return nil";

            Rewrite(code, expected, r => r.ReplaceReturnType("void"));
        }

        [Fact]
        public void ReplaceReturnTypeIntWithVoidTest()
        {
            var code = @"function Dummy() as int strict
return nil";

            var expected = @"function Dummy() as string strict
return nil";

            Rewrite(code, expected, r => r.ReplaceReturnType("string"));
        }

        [Fact]
        public void DeleteReturnTypeTest()
        {
            var code = @"procedure Dummy() as void
return nil";

            var expected = @"procedure Dummy()
return nil";

            Rewrite(code, expected, r => r.DeleteReturnType());
        }

        [Fact]
        public void ReplaceCallingConventionTest()
        {
            var code = @"procedure Dummy() as void strict
return nil";

            var expected = @"procedure Dummy() as void clipper
return nil";

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }

        [Fact]
        public void DeleteCallingConventionTest()
        {
            var code = @"procedure Dummy() as void strict
return nil";

            var expected = @"procedure Dummy() as void
return nil";

            Rewrite(code, expected, r => r.DeleteCallingConvention());
        }

    }
}
