using FluentAssertions;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;

namespace XSharp.Parser.Helpers.Tests.TestHelpers
{
    public class TestsFor<T> where T : IParseTree
    {
        protected string WrapInClass(string code)
    => $@"class dtaDummy
{code}
end class";

        protected string WrapInInterface(string code)
    => $@"interface dtaDummy
{code}
end interface";

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
            foreach (var item in parser.Tree.WhereType<T>())
                rewriteAction(parser.Tree.RewriterFor(item));

            var resultLines = parser.Tree.Rewriter.GetText().SplitLines();
            var expectedLines = expected.SplitLines();
            for (var i = 0; i < resultLines.Length; i++)
                resultLines[i].Should().Be(expectedLines.ElementAtOrDefault(i), $"Line {i}");
        }

        protected T GetFirst(string code)
        {
            var parser = code.ParseText();
            return parser.Tree.FirstOrDefaultType<T>();
        }

        protected IEnumerable<T> GetAll(string code)
        {
            var parser = code.ParseText();
            return parser.Tree.WhereType<T>();
        }
    }
}
