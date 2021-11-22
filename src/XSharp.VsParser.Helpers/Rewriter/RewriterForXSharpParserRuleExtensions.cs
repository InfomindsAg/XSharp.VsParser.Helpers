using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForXSharpParserRule Extensions
    /// </summary>
    public static class RewriterForXSharpParserRuleExtensions
    {

        /// <summary>
        /// Replaces the token
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newContent">The new content</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<T> Replace<T>(this RewriterForContext<T> rewriterFor, string newContent) where T : XSharpParserRuleContext
        {
            if (rewriterFor.Context == null)
                throw new ArgumentException("Context can not be empty");

            rewriterFor.Rewriter.Replace(rewriterFor.Context.start.ToIndex(), rewriterFor.Context.stop.ToIndex(), newContent);
            return rewriterFor;
        }
    }
}
