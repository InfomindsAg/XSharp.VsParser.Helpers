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
        /// Replaces all the arguments from the method call with new arguments
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newArguments">The new arguments</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodCallContext> ReplaceAllArguments(this RewriterForContext<MethodCallContext> rewriterFor, string newArguments)
        {
            var argList = rewriterFor.Context.ArgList;
            if (argList != null)
                rewriterFor.Rewriter.Replace(argList.Start.ToIndex(), argList.Stop.ToIndex(), newArguments);
            else
                rewriterFor.Rewriter.Replace(rewriterFor.Context.LPAREN().ToIndex(), rewriterFor.Context.RPAREN().ToIndex(), $"({newArguments})");
            return rewriterFor;
        }


        /// <summary>
        /// Replaces the called method name with a new method name, whenn the method called a member
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodCallContext> ReplaceAccessMemberMethodName(this RewriterForContext<MethodCallContext> rewriterFor, string newMethodName)
        {
            var acccesMember = rewriterFor.Context.ToValues()?.AccessMember?.Context;
            if (acccesMember?.Name != null)
                rewriterFor.RewriterFor(acccesMember).ReplaceMemberName(newMethodName);
            else
                throw new ArgumentException("MethodCall is not using AccessMember");

            return rewriterFor;
        }

        /// <summary>
        /// Replaces the called method name with a new method name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newName">The new method name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodCallContext> ReplaceNameExpression(this RewriterForContext<MethodCallContext> rewriterFor, string newName)
        {
            var NameExpression = rewriterFor.Context.ToValues()?.NameExpression?.Context;
            if (NameExpression?.Name != null)
                rewriterFor.RewriterFor(NameExpression).ReplaceMemberName(newName);
            else
                throw new ArgumentException("MethodCall is not using NameExpression");

            return rewriterFor;
        }
    }
}
