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
    public class CommonLocalDeclContextValuesTests : TestsFor<CommonLocalDeclContext>
    {
        [Fact]
        public void OneNoTypeTest()
        {
            var code = WrapInMethod(@"local dummy");

            GetFirst(code).ToValues().Variables.Select(q => q.Type).Should().BeEquivalentTo(new string[] { null });
        }

        [Fact]
        public void TwoNoTypeTest()
        {
            var code = WrapInMethod(@"local dummy1, dummy2");

            GetFirst(code).ToValues().Variables.Select(q => q.Type).Should().BeEquivalentTo(new string[] { null, null });
        }


        [Fact]
        public void OneWithTypeTest()
        {
            var code = WrapInMethod(@"local dummy as string");

            GetFirst(code).ToValues().Variables.Select(q => q.Type).Should().BeEquivalentTo(new string[] { "string" });
        }

        [Fact]
        public void TwoWithTypeTest()
        {
            var code = WrapInMethod(@"local dummy1, dummy2 as string");

            GetFirst(code).ToValues().Variables.Select(q => q.Type).Should().BeEquivalentTo(new string[] { "string", "string" });
        }

        [Fact]
        public void ComplexWithTypeTest()
        {
            var code = WrapInMethod(@"local lOk as logic, nAnzRec as dword, nOK, nError as dword, cWarnungen, cText as string");

            GetFirst(code).ToValues().Variables.Select(q => q.Type).Should().BeEquivalentTo(new string[] { "logic", "dword", "dword", "dword", "string", "string" });
        }

    }
}
