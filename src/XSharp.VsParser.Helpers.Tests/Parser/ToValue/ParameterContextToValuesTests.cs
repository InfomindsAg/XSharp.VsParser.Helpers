using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class ParameterContextToValuesTests : TestsFor<MethodContext>
    {
        [Fact]
        public void ParametersTest()
        {
            var code = WrapInClass(@"method Dummy(text as string, number := 0 as int) as string strict
return nil");

            var result = GetFirst(code).ToValues().Parameters;
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new { Name = "text", DataType = "string" });
            result[1].Should().BeEquivalentTo(new { Name = "number", DataType = "int", Default = "0" });
        }

        [Fact]
        public void ParametersUntypedTest()
        {
            var code = WrapInClass(@"method Dummy(text, number)
return nil");

            var result = GetFirst(code).ToValues().Parameters;
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new { Name = "text", DataType = (string)null });
            result[1].Should().BeEquivalentTo(new { Name = "number", DataType = (string)null });
        }
    }
}
