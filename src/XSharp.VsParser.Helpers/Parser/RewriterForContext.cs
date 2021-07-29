using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    public class RewriterForContext<T> where T : IParseTree
    {
        public T Context { get; internal set; }
        public TokenStreamRewriter Rewriter { get; internal set; }

        public RewriterForContext(TokenStreamRewriter rewriter, T context)
        {
            Context = context;
            Rewriter = rewriter;
        }

        public RewriterForContext<N> RewriterFor<N>(N context) where N : IParseTree
            => new RewriterForContext<N>(Rewriter, context);
    }
}
