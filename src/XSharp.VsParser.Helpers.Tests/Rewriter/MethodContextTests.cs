using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class MethodContextTests : TestsFor<MethodContext>
    {
        [Fact]
        public void AddOverrideTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddOverride());
        }

        [Fact]
        public void AddOverrideWithPublicTest()
        {
            var code = WrapInClass(@"public method Dummy()
return nil");

            var expected = WrapInClass(@"public override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddOverride());
        }

        [Fact]
        public void ReplaceNoReturnTypeWithVoidTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"method Dummy() as void
return nil");

            Rewrite(code, expected, r => r.ReplaceReturnType("void"));
        }

        [Fact]
        public void ReplaceReturnTypeIntWithVoidTest()
        {
            var code = WrapInClass(@"method Dummy() as int strict
return nil");

            var expected = WrapInClass(@"method Dummy() as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceReturnType("void"));
        }

        [Fact]
        public void DeleteReturnTypeTest()
        {
            var code = WrapInClass(@"method Dummy() as void
return nil");

            var expected = WrapInClass(@"method Dummy()
return nil");

            Rewrite(code, expected, r => r.DeleteReturnType());
        }

        [Fact]
        public void ReplaceCallingConventionTest()
        {
            var code = WrapInClass(@"method Dummy() as void strict
return nil");

            var expected = WrapInClass(@"method Dummy() as void clipper
return nil");

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }

        [Fact]
        public void DeleteCallingConventionTest()
        {
            var code = WrapInClass(@"method Dummy() as void strict
return nil");

            var expected = WrapInClass(@"method Dummy() as void
return nil");

            Rewrite(code, expected, r => r.DeleteCallingConvention());
        }

    }
}
