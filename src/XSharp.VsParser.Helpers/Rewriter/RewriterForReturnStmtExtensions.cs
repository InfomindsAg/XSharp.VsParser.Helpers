using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForReturnStmt Extensions
    /// </summary>
    public static class RewriterForReturnStmtExtensions
    {
        /// <summary>
        /// Deletes the expression of the return statement
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ReturnStmtContext> DeleteExpression(this RewriterForContext<ReturnStmtContext> rewriterFor)
        {
            var expr = rewriterFor.Context.Expr;
            if ((expr?.ChildCount ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Delete(expr.Start.ToIndex(), expr.Stop.ToIndex());
            return rewriterFor;
        }
    }
}
