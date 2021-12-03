using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class ConstructorchainContextTests : TestsFor<ConstructorchainContext>
    {
        [Fact]
        public void DeleteAllParametersTest()
        {
            var code = WrapInConstructor(@"super(null_object)");

            var expected = WrapInConstructor(@"super()");

            Rewrite(code, expected, r => r.DeleteAllArguments());
        }

        [Fact]
        public void ReplaceAllParametersTest()
        {
            var code = WrapInConstructor(@"super(1, 2, 3)");

            var expected = WrapInConstructor(@"super(1, 3)");

            Rewrite(code, expected, r => r.ReplaceAllArguments("1, 3"));
        }

        [Fact]
        public void ReplaceAllParametersWhenEmptyTest()
        {
            var code = WrapInConstructor(@"super()");

            var expected = WrapInConstructor(@"super(1, 3)");

            Rewrite(code, expected, r => r.ReplaceAllArguments("1, 3"));
        }


    }
}
