using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForMethodCall Extensions
    /// </summary>
    public static class RewriterForNameExpressionExtensions
    {
        /// <summary>
        /// Replaces the name with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newName">The new name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<NameExpressionContext> ReplaceMemberName(this RewriterForContext<NameExpressionContext> rewriterFor, string newName)
        {
            var name = rewriterFor.Context?.Name;
            if (name != null)
                rewriterFor.Rewriter.Replace(name.Start.ToIndex(), name.Stop.ToIndex(), newName);

            return rewriterFor;
        }
    }
}
