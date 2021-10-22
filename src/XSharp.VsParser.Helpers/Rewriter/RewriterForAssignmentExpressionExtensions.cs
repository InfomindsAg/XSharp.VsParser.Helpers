using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForAssignmentExpression Extensions
    /// </summary>
    public static class RewriterForAssignmentExpressionExtensions
    {
        /// <summary>
        /// Replaces the property name with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<AssignmentExpressionContext> ReplacePropertyName(this RewriterForContext<AssignmentExpressionContext> rewriterFor, string newMethodName)
        {
            var acccesMember = rewriterFor.Context.ToValues()?.AssignToAccessMember?.Context;
            if (acccesMember?.Name != null)
                rewriterFor.RewriterFor(acccesMember).ReplaceMemberName(newMethodName);

            return rewriterFor;
        }

        /// <summary>
        /// Replaces the property name with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newValueExpression">The new name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<AssignmentExpressionContext> ReplaceValue(this RewriterForContext<AssignmentExpressionContext> rewriterFor, string newValueExpression)
        {
            var expr = rewriterFor.Context.Right;
            if (expr != null)
                rewriterFor.Rewriter.Replace(expr.start.ToIndex(), expr.start.ToIndex(), newValueExpression);

            return rewriterFor;
        }
    }
}
