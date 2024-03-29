﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.Parser.Helpers.Tests.TestHelpers;
using XSharp.VsParser.Helpers.Parser;
using Xunit;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Tests.Parser.ToValue
{
    public class NameExpressionToValuesTests : TestsFor<NameExpressionContext>
    {
        [Fact]
        public void SimpleNameTest()
        {
            var code = WrapInMethod(@"
Dummy()
return nil");

            GetFirst(code).ToValues().Should().BeEquivalentTo(new
            {
                Name = "Dummy",
            });
        }

    }
}
