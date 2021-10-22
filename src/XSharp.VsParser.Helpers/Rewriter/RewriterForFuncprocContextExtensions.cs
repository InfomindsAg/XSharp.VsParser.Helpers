using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForFuncprocContext Extensions
    /// </summary>
    public static class RewriterForFuncprocContextExtensions
    {
        static RewriterForContext<SignatureContext> RewriterForSignature(RewriterForContext<FuncprocContext> rewriterFor)
            => rewriterFor.RewriterFor(rewriterFor.Context.Sig);

        /// <summary>
        /// Replaces the name of the function/procedure with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method names</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> ReplaceMethodName(this RewriterForContext<FuncprocContext> rewriterFor, string newMethodName)
        {
            RewriterForSignature(rewriterFor).ReplaceMethodName(newMethodName);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes all the parameters of the function/procedure
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> DeleteAllParameters(this RewriterForContext<FuncprocContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteAllParameters();
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the existing parameters with new parameters
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newParameters">The new parameters</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> ReplaceParameters(this RewriterForContext<FuncprocContext> rewriterFor, string newParameters)
        {
            RewriterForSignature(rewriterFor).ReplaceParameters(newParameters);
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the return type of the function/procedure
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newReturnType">The new return type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> ReplaceReturnType(this RewriterForContext<FuncprocContext> rewriterFor, string newReturnType)
        {
            RewriterForSignature(rewriterFor).ReplaceReturnType(newReturnType);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the return type of the function/procedure
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> DeleteReturnType(this RewriterForContext<FuncprocContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteReturnType();
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the calling convention of the function/procedure 
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newCallingConvention">The new calling convention</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> ReplaceCallingConvention(this RewriterForContext<FuncprocContext> rewriterFor, string newCallingConvention)
        {
            RewriterForSignature(rewriterFor).ReplaceCallingConvention(newCallingConvention);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the calling convention of the function/procedure 
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<FuncprocContext> DeleteCallingConvention(this RewriterForContext<FuncprocContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteCallingConvention();
            return rewriterFor;
        }
    }
}
