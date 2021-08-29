using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class ParameterContextTests : TestsFor<ParameterContext>
    {

        [Fact]
        public void ChangeParamDataTypeTypedTest()
        {
            var code = WrapInClass(@"method Dummy(isValid as usual) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid as logic) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDataType("logic"));
        }

        [Fact]
        public void ChangeParamDataTypeTypedAndDefaultTest()
        {
            var code = WrapInClass(@"method Dummy(isValid := false as usual) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid := false as logic) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDataType("logic"));
        }


        [Fact]
        public void ChangeParamDataTypeUnypedTest()
        {
            var code = WrapInClass(@"method Dummy(isValid) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid as logic) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDataType("logic"));
        }

        [Fact]
        public void ChangeParamDataTypeUntypedAndDefaultTest()
        {
            var code = WrapInClass(@"method Dummy(isValid := false) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid := false as logic) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDataType("logic"));
        }

    }
}
