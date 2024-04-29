using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using Xunit;
using XSharp.VsParser.Helpers.Parser;
using FluentAssertions;

namespace XSharp.VsParser.Helpers.Tests.Parser.NamedArgumentsToValue
{
    public class MethodCallToValuesTest : TestsFor<MethodCallContext>
    {
        [Fact]
        public void MethodCallWithArguments()
        {
            var code = WrapInMethod("""
                        self:TabWindow(test := "x")
                        return nil
                        """);

            var result = GetAll(code, true).ToValues().ToArray();
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new
            {
                NameExpression = (object)null,
                AccessMember = new { AccessExpression = "self", MemberName = "TabWindow", },
                Arguments = new[] { new { Name = "test", Value = "\"x\"" } }
            });
        }
    }
}
