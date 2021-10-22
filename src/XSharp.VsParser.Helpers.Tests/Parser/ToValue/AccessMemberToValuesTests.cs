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
    public class AccessMemberToValuesTests : TestsFor<AccessMemberContext>
    {
        [Fact]
        public void SimpleNameTest()
        {
            var code = WrapInMethod(@"
self:Dummy := '15'
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                AccessExpression = "self",
                MemberName = "Dummy",
                IsSuperAccess = false,
            });
        }

        [Fact]
        public void SuperNameTest()
        {
            var code = WrapInMethod(@"
super:Dummy()
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                AccessExpression = "super",
                MemberName = "Dummy",
                IsSuperAccess = true,
            });
        }
    }
}
