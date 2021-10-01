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
    public static class RewriterForMethodCallExtensions
    {
        /// <summary>
        /// Deletes all the arguments from the method call
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodCallContext> DeleteAllArguments(this RewriterForContext<MethodCallContext> rewriterFor)
        {
            var argList = rewriterFor.Context.ArgList;
            if ((argList?.ChildCount ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Delete(argList.Start.ToIndex(), argList.Stop.ToIndex());
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the called method name with a new method name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodCallContext> ReplaceMethodName(this RewriterForContext<MethodCallContext> rewriterFor, string newMethodName)
        {
            var accessMember = rewriterFor.Context.Expr?.AsEnumerable().FirstOrDefaultType<AccessMemberContext>();
            if (accessMember != null && accessMember.Name != null)
                rewriterFor.Rewriter.Replace(accessMember.Name.Start.ToIndex(), accessMember.Name.Stop.ToIndex(), newMethodName);

            return rewriterFor;
        }
    }
}
