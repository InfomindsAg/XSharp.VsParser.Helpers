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
        public void ChangeParamNameTest()
        {
            var code = WrapInClass(@"method Dummy(valid as usual) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid as usual) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterName("isValid"));
        }

        [Fact]
        public void ChangeParamNameUntypedTest()
        {
            var code = WrapInClass(@"method Dummy(valid)
return nil");

            var expected = WrapInClass(@"method Dummy(isValid)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterName("isValid"));
        }

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
        public void ChangeParamWithTypeAddDefaultTest()
        {
            var code = WrapInClass(@"method Dummy(isValid as usual) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid := nil as usual) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDefaultValue("nil"));
        }

        [Fact]
        public void ChangeParamAddDefaultTest()
        {
            var code = WrapInClass(@"method Dummy(isValid) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid := nil) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDefaultValue("nil"));
        }


        [Fact]
        public void ChangeParamDefaultTest()
        {
            var code = WrapInClass(@"method Dummy(isValid := false as logic) as void strict
return nil");

            var expected = WrapInClass(@"method Dummy(isValid := true as logic) as void strict
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDefaultValue("true"));
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

        [Fact]
        public void ReplaceAssignParamTypeTest()
        {
            var code = WrapInClass(@"assign KeyDisplayString (value)
return nil");

            var expected = WrapInClass(@"assign KeyDisplayString (value as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameterDataType("string"));
        }

    }
}
