using LanguageService.SyntaxTree.Tree;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Base class for Values classes
    /// </summary>
    /// <typeparam name="T">The Context Type</typeparam>
    public class ContextValues<T> where T : IParseTree
    {
        /// <summary>
        /// The Context, from which the values were extracted
        /// </summary>
        public T Context { get; internal set; }
    }
}
