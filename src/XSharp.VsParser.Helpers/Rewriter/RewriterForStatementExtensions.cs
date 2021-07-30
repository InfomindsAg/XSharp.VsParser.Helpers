using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForStatementExtensions
    {

        #region ReturnStmt

        public static RewriterForContext<ReturnStmtContext> DeleteExpression(this RewriterForContext<ReturnStmtContext> rewriterFor)
        {
            var expr = rewriterFor.Context.Expr;
            if ((expr?.ChildCount ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Delete(expr.Start.ToIndex(), expr.Stop.ToIndex());
            return rewriterFor;
        }

        #endregion

        #region Statement

        public static RewriterForContext<StatementContext> ReplaceStatement(this RewriterForContext<StatementContext> rewriterFor, string newStatement)
        {
            if (!string.IsNullOrEmpty(newStatement) && !newStatement.EndsWith(Environment.NewLine))
                newStatement += Environment.NewLine;

            rewriterFor.Rewriter.Replace(rewriterFor.Context.start.ToIndex(), rewriterFor.Context.stop.ToIndex(), newStatement);
            return rewriterFor;
        }

        public static RewriterForContext<StatementContext> DeleteStatement(this RewriterForContext<StatementContext> rewriterFor)
        {
            rewriterFor.Rewriter.Delete(rewriterFor.Context.start.ToIndex(), rewriterFor.Context.stop.ToIndex());
            return rewriterFor;
        }

        #endregion

    }
}
