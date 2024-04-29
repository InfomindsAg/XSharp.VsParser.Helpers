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

            Rewrite(code, expected, r => r.AddModifiers("override"));
        }

        [Fact]
        public void AddOverrideWithPublicTest()
        {
            var code = WrapInClass(@"public method Dummy()
return nil");

            var expected = WrapInClass(@"public override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddModifiers("override"));
        }

        [Fact]
        public void AddProtectedOverrideTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"protected override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddModifiers("protected override"));
        }

        [Fact]
        public void AddProtectedOverrideExistingTest()
        {
            var code = WrapInClass(@"protected method Dummy()
return nil");

            var expected = WrapInClass(@"protected override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddModifiers("protected override"));
        }

        [Fact]
        public void AddProtectedOverrideExisting2Test()
        {
            var code = WrapInClass(@"override protected method Dummy()
return nil");

            var expected = WrapInClass(@"protected override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddModifiers("protected override"));
        }

        [Fact]
        public void DeletePublicTest()
        {
            var code = WrapInClass(@"public method Dummy()
return nil");

            var expected = WrapInClass(@"method Dummy()
return nil");

            Rewrite(code, expected, r => r.DeleteAllModifiers());
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
        public void ReplaceAccessNoReturnTypeWithUsualTest()
        {
            var code = WrapInClass(@"access Dummy
return nil");

            var expected = WrapInClass(@"access Dummy as usual
return nil");

            Rewrite(code, expected, r => r.ReplaceReturnType("usual"));
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
        public void ReplaceCallingConventionAccessTest()
        {
            var code = WrapInClass(@"access Dummy
return nil");

            var expected = WrapInClass(@"access Dummy clipper
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

        [Fact]
        public void DeleteAllParametersTest()
        {
            var code = WrapInClass(@"method Dummy(test) as void
return nil");

            var expected = WrapInClass(@"method Dummy() as void
return nil");

            Rewrite(code, expected, r => r.DeleteAllParameters());
        }

        [Fact]
        public void DeleteAllParametersWithNoParametersTest()
        {
            var code = WrapInClass(@"method Dummy() as void
return nil");

            var expected = WrapInClass(@"method Dummy() as void
return nil");

            Rewrite(code, expected, r => r.DeleteAllParameters());
        }

        [Fact]
        public void ReplaceParametersEmptyTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"method Dummy(newParam)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam"));
        }

        [Fact]
        public void ReplaceParametersTypedEmptyTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"method Dummy(newParam as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam as string"));
        }

        [Fact]
        public void ReplaceParametersTypedNoParamListTest()
        {
            var code = WrapInClass(@"method Dummy
return nil");

            var expected = WrapInClass(@"method Dummy(newParam as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam as string"));
        }

        [Fact]
        public void ReplaceParametersNotEmptyTest()
        {
            var code = WrapInClass(@"method Dummy(firstParam as string)
return nil");

            var expected = WrapInClass(@"method Dummy(firstParam as string, newParam as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("firstParam as string, newParam as string"));
        }

        [Fact]
        public void AddTwoParametersNotEmptyTest()
        {
            var code = WrapInClass(@"method Dummy(firstParam as string)
return nil");

            var expected = WrapInClass(@"method Dummy(firstParam as string, newParam1 as string, newParam2 as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("firstParam as string, newParam1 as string, newParam2 as string"));
        }

        [Fact]
        public void AddTwoParametersEmptyTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"method Dummy(newParam1 as string, newParam2 as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam1 as string, newParam2 as string"));
        }
    }
}
