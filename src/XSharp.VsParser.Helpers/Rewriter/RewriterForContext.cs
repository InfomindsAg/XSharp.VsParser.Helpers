using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// The RewriterForContext class. This class is used as an extension point for the RewriteFor Extensions
    /// </summary>
    /// <typeparam name="T">The context type</typeparam>
    public class RewriterForContext<T> where T : IParseTree
    {
        /// <summary>
        /// The context instance for the Rewriter
        /// </summary>
        public T Context { get; internal set; }

        /// <summary>
        /// The Rewriter instance
        /// </summary>
        public TokenStreamRewriter Rewriter { get; internal set; }

        internal RewriterForContext(TokenStreamRewriter rewriter, T context)
        {
            Context = context;
            Rewriter = rewriter;
        }

        /// <summary>
        /// Creates a new RewriterForContext instance
        /// </summary>
        /// <typeparam name="N">The new Type of the Context</typeparam>
        /// <param name="context">A Context instance</param>
        /// <returns>A new RewriterForContext instance </returns>
        public RewriterForContext<N> RewriterFor<N>(N context) where N : IParseTree
            => new(Rewriter, context);
    }
}
