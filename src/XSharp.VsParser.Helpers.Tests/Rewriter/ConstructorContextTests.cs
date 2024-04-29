using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class ConstructorContextTests : TestsFor<ConstructorContext>
    {

        [Fact]
        public void ReplaceCallingConventionTest()
        {
            var code = WrapInClass(@"Constructor () strict
return nil");

            var expected = WrapInClass(@"Constructor () clipper
return nil");

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }

        [Fact]
        public void ReplaceCallingConventionNoParamListTest()
        {
            var code = WrapInClass(@"Constructor strict
return nil");

            var expected = WrapInClass(@"Constructor clipper
return nil");

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }

        [Fact]
        public void ReplaceCallingConventionNoParamListNoConventionTest()
        {
            var code = WrapInClass(@"Constructor
return nil");

            var expected = WrapInClass(@"Constructor clipper
return nil");

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }


        [Fact]
        public void DeleteCallingConventionTest()
        {
            var code = WrapInClass(@"Constructor () strict
return nil");

            var expected = WrapInClass(@"Constructor ()
return nil");

            Rewrite(code, expected, r => r.DeleteCallingConvention());
        }

        [Fact]
        public void DeleteAllParametersTest()
        {
            var code = WrapInClass(@"Constructor(test)
return nil");

            var expected = WrapInClass(@"Constructor()
return nil");

            Rewrite(code, expected, r => r.DeleteAllParameters());
        }

        [Fact]
        public void DeleteAllParametersWithNoParametersTest()
        {
            var code = WrapInClass(@"Constructor()
super()
return nil");

            var expected = WrapInClass(@"Constructor()
super()
return nil");

            Rewrite(code, expected, r => r.DeleteAllParameters());
        }

        [Fact]
        public void ReplaceParametersEmptyTest()
        {
            var code = WrapInClass(@"Constructor ()
return nil");

            var expected = WrapInClass(@"Constructor (newParam)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam"));
        }

        [Fact]
        public void ReplaceParametersNoParamListTest()
        {
            var code = WrapInClass(@"Constructor
return nil");

            var expected = WrapInClass(@"Constructor(newParam)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam"));
        }

        [Fact]
        public void ReplaceParametersTypedEmptyTest()
        {
            var code = WrapInClass(@"Constructor ()
return nil");

            var expected = WrapInClass(@"Constructor (newParam as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("newParam as string"));
        }

        [Fact]
        public void ReplaceParametersNotEmptyTest()
        {
            var code = WrapInClass(@"Constructor (firstParam as string)
return nil");

            var expected = WrapInClass(@"Constructor (firstParam as string, newParam as string)
return nil");

            Rewrite(code, expected, r => r.ReplaceParameters("firstParam as string, newParam as string"));
        }

        [Fact]
        public void ConventionPlaygroundTest()
        {
            var code = @"class IndexFilterLandesbezeichnungen inherit IndexFilterSQL

CONSTRUCTOR(oDB)
SUPER(oDB)
return
end class"
;

            var expected = @"class IndexFilterLandesbezeichnungen inherit IndexFilterSQL

CONSTRUCTOR(oDB) clipper
SUPER(oDB)
return
end class"
;

            Rewrite(code, expected, r => r.ReplaceCallingConvention("clipper"));
        }

    }
}
