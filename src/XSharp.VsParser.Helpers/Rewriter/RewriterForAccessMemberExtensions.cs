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
    public static class RewriterForAccessMemberExtensions
    {
        /// <summary>
        /// Replaces the member name with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMemberName">The new member name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<AccessMemberContext> ReplaceMemberName(this RewriterForContext<AccessMemberContext> rewriterFor, string newMemberName)
        {
            var name = rewriterFor.Context?.Name;
            if (name != null)
                rewriterFor.Rewriter.Replace(name.Start.ToIndex(), name.Stop.ToIndex(), newMemberName);

            return rewriterFor;
        }
    }
}
