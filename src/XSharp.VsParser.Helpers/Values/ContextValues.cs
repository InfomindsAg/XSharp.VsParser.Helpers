using LanguageService.SyntaxTree.Tree;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Values
{
    public class ContextValues<T> where T : IParseTree
    {
        public T Context { get; internal set; }
    }
}
