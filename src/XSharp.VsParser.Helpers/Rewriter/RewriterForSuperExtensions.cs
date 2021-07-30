using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForSuperExtensions
    {

        #region SuperExpression

        public static RewriterForContext<SuperExpressionContext> ReplaceSuperCallMethodName(this RewriterForContext<SuperExpressionContext> rewriterFor, string newMethodName)
        {
            var accessMemberContext = rewriterFor.Context.FirstParentOrDefault<AccessMemberContext>().AsEnumerable();
            var methodNameContext = accessMemberContext.FirstOrDefaultType<SimpleNameContext>();
            rewriterFor.RewriterFor(methodNameContext.Id).ReplaceIdentifier(newMethodName);
            return rewriterFor;
        }

        public static RewriterForContext<SuperExpressionContext> DeleteAllArguments(this RewriterForContext<SuperExpressionContext> rewriterFor)
        {
            rewriterFor.RewriterFor(rewriterFor.Context.FirstParentOrDefault<MethodCallContext>()).DeleteAllArguments();
            return rewriterFor;
        }

        #endregion

    }
}
