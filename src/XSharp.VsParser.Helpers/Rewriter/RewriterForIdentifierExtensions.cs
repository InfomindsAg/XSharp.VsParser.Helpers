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
    public static class RewriterForIdentifierExtensions
    {
        /// <summary>
        /// Replaces an identifier with a new identifier
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newIdentifier">The new idenifier</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<IdentifierContext> ReplaceIdentifier(this RewriterForContext<IdentifierContext> rewriterFor, string newIdentifier)
        {
            if (!string.Equals(rewriterFor.Context?.GetText(), newIdentifier))
                rewriterFor.Rewriter.Replace(rewriterFor.Context.ToIndex(), newIdentifier);
            return rewriterFor;
        }
    }
}
