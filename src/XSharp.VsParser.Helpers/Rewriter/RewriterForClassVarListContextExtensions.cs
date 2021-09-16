using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForClassVarListContext Extensions
    /// </summary>
    public static class RewriterForClassVarListContextExtensions
    {

        /// <summary>
        /// Replaces the type of the class var list
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newType">The new return type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ClassVarListContext> ReplaceType(this RewriterForContext<ClassVarListContext> rewriterFor, string newType)
        {
            if (string.IsNullOrEmpty(newType))
                throw new ArgumentException($"{nameof(newType)} can not be empty");

            var typeContext = rewriterFor.Context.DataType;
            if (typeContext != null)
                rewriterFor.Rewriter.Replace(typeContext.start.ToIndex(), typeContext.stop.ToIndex(), InternalRewriterHelper.RemoveAsFromType(newType));
            else
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.stop.ToIndex(), " " + InternalRewriterHelper.AddAsToType(newType));
            return rewriterFor;
        }
    }
}
