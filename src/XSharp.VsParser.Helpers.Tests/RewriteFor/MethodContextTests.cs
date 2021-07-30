using System;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.RewriteFor
{
    public class MethodContextTests : RewriteForTests<MethodContext>
    {
        [Fact]
        public void AddOverride()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            var expected = WrapInClass(@"override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddOverride());
        }

        [Fact]
        public void AddOverrideWithPublic()
        {
            var code = WrapInClass(@"public method Dummy()
return nil");

            var expected = WrapInClass(@"public override method Dummy()
return nil");

            Rewrite(code, expected, r => r.AddOverride());
        }


    }
}
