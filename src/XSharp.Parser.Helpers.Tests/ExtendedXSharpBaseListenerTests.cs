using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XSharp.Parser.Helpers;
using static IM.DevTools.XsFormToWinForm.Parser.Tests.TestFileName;
using XSharp.Parser.Helpers.Listeners;
using LanguageService.SyntaxTree.Misc;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;

namespace IM.DevTools.XsFormToWinForm.Parser.Tests
{
    public class ExtendedXSharpBaseListenerTests
    {
        class TestListener : ExtendedXSharpBaseListener
        {
            public List<string> Methods = new();

            public override void EnterMethod([NotNull] XSharpParser.MethodContext context)
            {
                base.EnterMethod(context);

                if (Current.Class != null && Current.Method != null)
                    Methods.Add($"{Current.Class.Name}.{Current.Method.Name}");
            }
        }

        [Fact]
        public void Context()
        {
            var parser = ParserHelper.BuildWithVoDefaultOptions();
            var result = parser.ParseFile(CodeFile("StringBuilderExamples.prg"));
            result.Should().NotBeNull();
            result.OK.Should().BeTrue();

            var testListener = new TestListener();
            parser.ExecuteListeners(new List<XSharpBaseListener> { testListener });

            testListener.Methods.Should().Contain(
                new List<string>() {
                "StringBuilderExamples.Execute",
                "StringBuilderExamples.ConcatenateNoLineBreaks",
                "StringBuilderExamples.ConcatenateWithLineBreaks",
                "StringBuilderExamples.FluentApi",
                "StringBuilderExamples.FluentApiMultiLine",
                "StringBuilderExamples.Clear",
                "StringBuilderExamples.AppendFormat",
                "StringBuilderExamples.InsertAndRemove",
                });
        }

    }
}
