using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForSuperExpression Extensions
    /// </summary>
    public static class RewriterForSuperExpressionExtensions
    {

        /// <summary>
        /// Replaces the name of method called in the super expression with a new method name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SuperExpressionContext> ReplaceSuperCallMethodName(this RewriterForContext<SuperExpressionContext> rewriterFor, string newMethodName)
        {
            var accessMemberContext = rewriterFor.Context.FirstParentOrDefault<AccessMemberContext>().AsEnumerable();
            var methodNameContext = accessMemberContext.FirstOrDefaultType<SimpleNameContext>();
            rewriterFor.RewriterFor(methodNameContext.Id).ReplaceIdentifier(newMethodName);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes all agruments of the method call in the super expression
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SuperExpressionContext> DeleteAllArguments(this RewriterForContext<SuperExpressionContext> rewriterFor)
        {
            rewriterFor.RewriterFor(rewriterFor.Context.FirstParentOrDefault<MethodCallContext>()).DeleteAllArguments();
            return rewriterFor;
        }

    }
}
