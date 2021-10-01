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
    public class MethodCallToValuesTests : TestsFor<MethodCallContext>
    {
        [Fact]
        public void SimpleNameTest()
        {
            var code = WrapInMethod(@"
self:Dummy(null_object)
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                Expression = "self",
                MethodName = "Dummy",
                Arguments = new[] { new { Value = "null_object" } }
            });
        }

        [Fact]
        public void ComplexNameTest()
        {
            var code = WrapInMethod(@"
self:TabWindow():Dummy(null_object)
return nil");

            GetAll(code).ToValues().Should().BeEquivalentTo(
                new
                {
                    Expression = "self",
                    MethodName = "TabWindow",
                    Arguments = new object[0]
                },
                new
                {
                    Expression = "self:TabWindow()",
                    MethodName = "Dummy",
                    Arguments = new[] { new { Value = "null_object" }
                    }
                });
        }
    }
}
