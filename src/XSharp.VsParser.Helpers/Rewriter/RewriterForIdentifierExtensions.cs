using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForIdentifierExtensions
    {
        #region Identifier 

        public static RewriterForContext<IdentifierContext> ReplaceIdentifier(this RewriterForContext<IdentifierContext> rewriterFor, string newIdentifier)
        {
            if (!string.Equals(rewriterFor.Context?.GetText(), newIdentifier))
                rewriterFor.Rewriter.Replace(rewriterFor.Context.ToIndex(), newIdentifier);
            return rewriterFor;
        }

        #endregion
    }
}
