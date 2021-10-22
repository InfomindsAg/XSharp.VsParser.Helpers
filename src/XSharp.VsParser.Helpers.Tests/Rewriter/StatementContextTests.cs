using System;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Rewriter
{
    public class StatementContextTests : TestsFor<StatementContext>
    {
        [Fact]
        public void ReplaceStatementTest()
        {
            var code = WrapInMethod(@"return nil");

            var expected = WrapInMethod(@"// return nil");

            Rewrite(code, expected, r => r.ReplaceStatement("// return nil"));
        }

        [Fact]
        public void DeleteStatementTest()
        {
            var code = WrapInMethod(@"return nil");

            var expected = WrapInMethod(@"");

            Rewrite(code, expected, r => r.DeleteStatement());
        }


    }
}
