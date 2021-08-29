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
    public static class RewriterForStatementExtensions
    {
        /// <summary>
        /// Replaces the statement with a new statement
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>#
        /// <param name="newStatement">The new statement</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<StatementContext> ReplaceStatement(this RewriterForContext<StatementContext> rewriterFor, string newStatement)
        {
            if (!string.IsNullOrEmpty(newStatement) && !newStatement.EndsWith(Environment.NewLine))
                newStatement += Environment.NewLine;

            rewriterFor.Rewriter.Replace(rewriterFor.Context.start.ToIndex(), rewriterFor.Context.stop.ToIndex(), newStatement);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes a statement
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<StatementContext> DeleteStatement(this RewriterForContext<StatementContext> rewriterFor)
        {
            rewriterFor.Rewriter.Delete(rewriterFor.Context.start.ToIndex(), rewriterFor.Context.stop.ToIndex());
            return rewriterFor;
        }

    }
}
