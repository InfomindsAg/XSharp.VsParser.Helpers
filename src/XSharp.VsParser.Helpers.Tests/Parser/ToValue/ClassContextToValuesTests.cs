using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class ClassContextToValuesTests : TestsFor<Class_Context>
    {
        [Fact]
        public void NameTest()
        {
            var code = @"class Test inherit BaseTest
end class";

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "Test", Inherits = "BaseTest", Implements = new string[0] });
        }

        [Fact]
        public void InterfaceTest()
        {
            var code = @"class Test implements IInterface1, IInterface2
end class";

            GetFirst(code).ToValues().Should().BeEquivalentTo(new { Name = "Test", Inherits = (string)null, Implements = new string[] { "IInterface1", "IInterface2" } });
        }

    }
}
