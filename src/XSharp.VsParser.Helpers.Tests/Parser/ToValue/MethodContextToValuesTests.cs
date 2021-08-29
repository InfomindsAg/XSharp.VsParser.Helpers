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
    public class MethodContextToValuesTests : TestsFor<MethodContext>
    {
        [Fact]
        public void NameTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            GetFirst(code).ToValues().Name.Should().Be("Dummy");
        }

        [Fact]
        public void ReturnTypeClipperTest()
        {
            var code = WrapInClass(@"method Dummy()
return nil");

            GetFirst(code).ToValues().ReturnType.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ReturnTypeStringTest()
        {
            var code = WrapInClass(@"method Dummy() as string strict
return nil");

            GetFirst(code).ToValues().ReturnType.Should().Be("string");
        }

        [Fact]
        public void CallingConvetionStrictTest()
        {
            var code = WrapInClass(@"method Dummy() as string strict
return nil");

            GetFirst(code).ToValues().CallingConvetion.Should().Be("strict");
        }
    }
}
