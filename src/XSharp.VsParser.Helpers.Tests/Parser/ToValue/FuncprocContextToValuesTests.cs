using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Parser.Values;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class FuncprocContextToValuesTests : TestsFor<FuncprocContext>
    {
        [Fact]
        public void NameTest()
        {
            var code = @"Procedure Dummy()
return nil";

            GetFirst(code).ToValues().Name.Should().Be("Dummy");
        }

        [Fact]
        public void ReturnTypeClipperTest()
        {
            var code = @"Procedure Dummy()
return nil";

            GetFirst(code).ToValues().ReturnType.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ReturnTypeStringTest()
        {
            var code = @"Procedure Dummy() as string strict
return nil";

            GetFirst(code).ToValues().ReturnType.Should().Be("string");
        }

        [Fact]
        public void CallingConvetionStrictTest()
        {
            var code = @"Procedure Dummy() as string strict
return nil";

            GetFirst(code).ToValues().CallingConvention.Should().Be("strict");
        }

        [Fact]
        public void ProcedureTest()
        {
            var code = @"Public Procedure Dummy()
return nil";

            GetFirst(code).ToValues().ProcFuncType.Should().Be(ProcFuncType.Procedure);
        }

        [Fact]
        public void FunctionTest()
        {
            var code = @"Public Function Dummy()
return nil";

            GetFirst(code).ToValues().ProcFuncType.Should().Be(ProcFuncType.Function);
        }

    }
}
