using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForIdentifier Extensions
    /// </summary>
    public static class RewriterForConstructorExtensions
    {
        static RewriterForContext<ParameterListContext> RewriterFoParameterList(RewriterForContext<ConstructorContext> rewriterFor)
                    => rewriterFor.RewriterFor(rewriterFor.Context?.ParamList);

        /// <summary>
        /// Deletes all the parameters of the construcor
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorContext> DeleteAllParameters(this RewriterForContext<ConstructorContext> rewriterFor)
        {
            RewriterFoParameterList(rewriterFor).DeleteAllParameters();
            return rewriterFor;
        }

        /// <summary>
        /// Adds a new parameters at the end of the parameterlist
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newParameter">The new parameter</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorContext> AddParameter(this RewriterForContext<ConstructorContext> rewriterFor, string newParameter)
        {
            RewriterFoParameterList(rewriterFor).AddParameter(newParameter);
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the calling convention of the constructor 
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newCallingConvention">The new calling convention</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorContext> ReplaceCallingConvention(this RewriterForContext<ConstructorContext> rewriterFor, string newCallingConvention)
        {
            var con = rewriterFor.Context;
            var lastToken = con.ParamList?.Stop ?? con.identifier()?.Stop;
            var index = lastToken?.ToIndex() ?? con.c1.ToIndex(); 
            InternalRewriterHelper.ReplaceCallingConvention(rewriterFor.Rewriter, newCallingConvention, rewriterFor.Context.callingconvention(), index);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the calling convention
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorContext> DeleteCallingConvention(this RewriterForContext<ConstructorContext> rewriterFor)
        {
            InternalRewriterHelper.DeleteCallingConvention(rewriterFor.Rewriter, rewriterFor.Context.callingconvention());
            return rewriterFor;
        }

    }
}
