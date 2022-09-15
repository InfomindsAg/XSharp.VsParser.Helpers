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
                NameExpression = (object)null,
                AccessMember = new { AccessExpression = "self", MemberName = "Dummy", },
                Arguments = new[] { new { Value = "null_object" } }
            });
        }

        [Fact]
        public void ComplexNameTest()
        {
            var code = WrapInMethod(@"
self:TabWindow():Dummy(null_object)
return nil");

            var result = GetAll(code).ToValues().ToArray();
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new
            {
                NameExpression = (object)null,
                AccessMember = new { AccessExpression = "self:TabWindow()", MemberName = "Dummy", },
                Arguments = new[] { new { Value = "null_object" } }
            });
            result[1].Should().BeEquivalentTo(new
            {
                NameExpression = (object)null,
                AccessMember = new { AccessExpression = "self", MemberName = "TabWindow", },
                Arguments = new object[0]
            });
        }

        [Fact]
        public void FuctionNameTest()
        {
            var code = WrapInMethod(@"
IsInstanceOf(null_object, #SingleLineEdit)
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                NameExpression = new { Name = "IsInstanceOf" },
                AccessMember = (object)null,
                Arguments = new[] { new { Value = "null_object" }, new { Value = "#SingleLineEdit" }, }
            });
        }

        [Fact]
        public void FuctionNameInIfTest()
        {
            var code = WrapInMethod(@"
if IsInstanceOf(null_object, #SingleLineEdit)
    nop
endif
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                NameExpression = new { Name = "IsInstanceOf" },
                AccessMember = (object)null,
                Arguments = new[] { new { Value = "null_object" }, new { Value = "#SingleLineEdit" }, }
            });
        }
    }
}
