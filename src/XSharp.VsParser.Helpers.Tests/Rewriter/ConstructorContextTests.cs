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
        public void AddParameterEmptyTest()
        {
            var code = WrapInClass(@"Constructor ()
return nil");

            var expected = WrapInClass(@"Constructor (newParam)
return nil");

            Rewrite(code, expected, r => r.AddParameter("newParam"));
        }

        [Fact]
        public void AddParameterTypedEmptyTest()
        {
            var code = WrapInClass(@"Constructor ()
return nil");

            var expected = WrapInClass(@"Constructor (newParam as string)
return nil");

            Rewrite(code, expected, r => r.AddParameter("newParam as string"));
        }

        [Fact]
        public void AddParameterNotEmptyTest()
        {
            var code = WrapInClass(@"Constructor (firstParam as string)
return nil");

            var expected = WrapInClass(@"Constructor (firstParam as string, newParam as string)
return nil");

            Rewrite(code, expected, r => r.AddParameter("newParam as string"));
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
