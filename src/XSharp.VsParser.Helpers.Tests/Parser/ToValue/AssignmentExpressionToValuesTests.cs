using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class AssignmentExpressionToValuesTests : TestsFor<AssignmentExpressionContext>
    {
        [Fact]
        public void SimpleNameTest()
        {
            var code = WrapInMethod(@"
self:Dummy := '15'
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                PropertyAccessExpression = "self",
                PropertyName = "Dummy",
                ValueExpression = "'15'",
            });
        }
    }
}
