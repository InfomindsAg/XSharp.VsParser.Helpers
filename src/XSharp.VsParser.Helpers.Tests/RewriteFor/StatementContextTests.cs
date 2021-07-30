using System;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.RewriteFor
{
    public class StatementContextTests : RewriteForTests<StatementContext>
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
