using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForPropertyClassVars Extensions
    /// </summary>
    public static class RewriterForClassvarsContextExtensions
    {

        /// <summary>
        /// Replaces the type of the class vars
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newType">The new type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ClassvarsContext> ReplaceType(this RewriterForContext<ClassvarsContext> rewriterFor, string newType)
        {
            if (string.IsNullOrEmpty(newType))
                throw new ArgumentException($"{nameof(newType)} can not be empty");

            var classVarList = rewriterFor.Context.classVarList();
            var typeContext = classVarList.DataType;
            if (typeContext != null)
                rewriterFor.Rewriter.Replace(typeContext.start.ToIndex(), typeContext.stop.ToIndex(), InternalRewriterHelper.RemoveAsFromType(newType));
            else
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.classVarList().stop.ToIndex(), " " + InternalRewriterHelper.AddAsToType(newType));
            return rewriterFor;
        }
    }
}
