using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForParameterList Extensions
    /// </summary>
    public static class RewriterForParameterListExtensions
    {
        /// <summary>
        /// Deletes all the parameters of the parameterlist
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ParameterListContext> DeleteAllParameters(this RewriterForContext<ParameterListContext> rewriterFor)
        {
            var paramList = rewriterFor.Context;
            if ((paramList?._Params?.Count ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Replace(paramList.Start.ToIndex(), paramList.Stop.ToIndex(), "()");
            return rewriterFor;
        }

        /// <summary>
        /// Adds a new parameters at the end of the parameterlist
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newParameter">The new parameter</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ParameterListContext> AddParameter(this RewriterForContext<ParameterListContext> rewriterFor, string newParameter)
        {
            var paramList = rewriterFor.Context;
            var separator = paramList.AsEnumerable().FirstOrDefaultType<ParameterContext>() != null ? ", " : "";
            rewriterFor.Rewriter.Replace(paramList.Stop.ToIndex(), paramList.Stop.ToIndex(), separator + newParameter + ")");
            return rewriterFor;
        }
    }
}
