using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForMethodContext Extensions
    /// </summary>
    public static class RewriterForMethodContextExtensions
    {
        static RewriterForContext<SignatureContext> RewriterForSignature(RewriterForContext<MethodContext> rewriterFor)
            => rewriterFor.RewriterFor(rewriterFor.Context.Sig);

        /// <summary>
        /// Replaces the name of the method with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method names</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> ReplaceMethodName(this RewriterForContext<MethodContext> rewriterFor, string newMethodName)
        {
            RewriterForSignature(rewriterFor).ReplaceMethodName(newMethodName);
            return rewriterFor;
        }

        /// <summary>
        /// Delates all the parameters of the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteAllParameters(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteAllParameters();
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the return type of the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newReturnType">The new return type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> ReplaceReturnType(this RewriterForContext<MethodContext> rewriterFor, string newReturnType)
        {
            RewriterForSignature(rewriterFor).ReplaceReturnType(newReturnType);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the return type of the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteReturnType(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteReturnType();
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the calling convention of the method 
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newCallingConvention">The new calling convention</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> ReplaceCallingConvention(this RewriterForContext<MethodContext> rewriterFor, string newCallingConvention)
        {
            RewriterForSignature(rewriterFor).ReplaceCallingConvention(newCallingConvention);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the calling convention
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteCallingConvention(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteCallingConvention();
            return rewriterFor;
        }

        /// <summary>
        /// Add Override to the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> AddOverride(this RewriterForContext<MethodContext> rewriterFor)
        {
            var modifiers = rewriterFor.Context.Modifiers;
            if (modifiers == null || modifiers.OVERRIDE().Length == 0)
                rewriterFor.Rewriter.InsertBefore(rewriterFor.Context.methodtype(0).Token.ToIndex(), "override ");
            return rewriterFor;
        }
    }
}
