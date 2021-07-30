using FluentAssertions;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;

namespace XSharp.Parser.Helpers.Tests.RewriteFor
{
    public class RewriteForTests<T> where T : IParseTree
    {
        protected string WrapInClass(string code)
    => $@"class dtaDummy
{code}
end class";

        protected string WrapInMethod(string code)
        {
            if (!string.IsNullOrEmpty(code))
                return WrapInClass($@"method Dummy()
{code}
");
            else
                return WrapInClass($@"method Dummy()
");
        }

        protected void Rewrite(string code, string expected, Action<RewriterForContext<T>> rewriteAction)
        {
            var parser = code.ParseText();
            foreach (var item in parser.SourceTree.WhereType<T>())
                rewriteAction(parser.SourceTree.RewriterFor(item));

            var resultLines = parser.SourceTree.Rewriter.GetText().SplitLines();
            var expectedLines = expected.SplitLines();
            for (var i = 0; i < resultLines.Length; i++)
                resultLines[i].Should().Be(expectedLines.ElementAtOrDefault(i), $"Line {i}");
        }
    }
}
