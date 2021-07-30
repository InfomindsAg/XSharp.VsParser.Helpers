using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Values;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests
{
    public class ToValuesTests
    {
        [Fact]
        public void ClassContextToValue()
        {
            var code = @"
class xxx inherit yyy
end class";

            code.ParseText().SourceTree.FirstOrDefaultType<Class_Context>().ToValues().Should().BeEquivalentTo(new { Name = "xxx", Inherits = "yyy" });
        }


        [Fact]
        public void MethodContextToValue()
        {
            var code = @"
class xxx
  method test() as void strict
  return
end class";

            code.ParseText().SourceTree.FirstOrDefaultType<MethodContext>().ToValues().Should().BeEquivalentTo(new { Name = "test", ReturnType = "void", CallingConvetion = "strict" });
        }

        [Fact]
        public void SuperExpressionContextToValue()
        {
            var code = @"
class xxx
  method test() as void strict
  return super:Test()
end class";

            code.ParseText().SourceTree.FirstOrDefaultType<SuperExpressionContext>().ToValues().Should().BeEquivalentTo(new { MethodName = "Test" });
        }



    }
}
