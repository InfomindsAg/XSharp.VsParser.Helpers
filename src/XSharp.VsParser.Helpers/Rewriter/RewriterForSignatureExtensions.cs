using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForSignature Extensions
    /// </summary>
    public static class RewriterForSignatureExtensions
    {
        /// <summary>
        /// Replaces the name of the signature with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newMethodName">The new method names</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> ReplaceMethodName(this RewriterForContext<SignatureContext> rewriterFor, string newMethodName)
        {
            new RewriterForContext<IdentifierContext>(rewriterFor.Rewriter, rewriterFor.Context.Id).ReplaceIdentifier(newMethodName);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes all the parameters of the signature
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> DeleteAllParameters(this RewriterForContext<SignatureContext> rewriterFor)
        {
            InternalRewriterHelper.DeleteAllParameters(rewriterFor.Rewriter, rewriterFor.Context.ParamList);
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the existing parameters with new parameters
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newParameters">The new parameters</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> ReplaceParameters(this RewriterForContext<SignatureContext> rewriterFor, string newParameters)
        {
            // TODO:!
            InternalRewriterHelper.ReplaceParameters(rewriterFor.Rewriter, newParameters, rewriterFor.Context.ParamList, rewriterFor.Context.Start.ToIndex());
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the return type of the signature
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newReturnType">The new return type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> ReplaceReturnType(this RewriterForContext<SignatureContext> rewriterFor, string newReturnType)
        {
            if (string.IsNullOrEmpty(newReturnType))
                throw new ArgumentException($"{nameof(newReturnType)} can not be empty");

            var returnContext = rewriterFor.Context.Type;
            if (returnContext != null)
                rewriterFor.Rewriter.Replace(returnContext.start.ToIndex(), returnContext.stop.ToIndex(), InternalRewriterHelper.RemoveAsFromType(newReturnType));
            else
            {
                newReturnType = " " + InternalRewriterHelper.AddAsToType(newReturnType);
                if (rewriterFor.Context.ParamList != null)
                    rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.ParamList.Stop.ToIndex(), newReturnType);
                else
                    rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.Id.Stop.ToIndex(), newReturnType);
            }
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the return type of the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> DeleteReturnType(this RewriterForContext<SignatureContext> rewriterFor)
        {
            var returnContext = rewriterFor.Context.Type;
            if (returnContext != null)
            {
                var previousChild = returnContext.RelativePositionedChildInParentOrDefault(-1);
                if (previousChild is TerminalNodeImpl asTerminal && string.Equals(asTerminal.GetText(), "as", StringComparison.OrdinalIgnoreCase))
                    rewriterFor.Rewriter.Delete(asTerminal.Symbol.ToIndex(), returnContext.stop.ToIndex());
                else
                    rewriterFor.Rewriter.Delete(returnContext.start.ToIndex(), returnContext.stop.ToIndex());
            }
            return rewriterFor;
        }

        /// <summary>
        /// Replaces the calling convention of the method 
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newCallingConvention">The new calling convention</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> ReplaceCallingConvention(this RewriterForContext<SignatureContext> rewriterFor, string newCallingConvention)
        {
            var sig = rewriterFor.Context;
            var lastToken = sig._ConstraintsClauses?.LastOrDefault()?.Stop ?? sig.Type?.Stop ?? sig.ParamList?.Stop ?? sig.TypeParameters?.Stop ?? sig.Id?.Stop;
            InternalRewriterHelper.ReplaceCallingConvention(rewriterFor.Rewriter, newCallingConvention, rewriterFor.Context.callingconvention(), lastToken.ToIndex());
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the calling convention
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> DeleteCallingConvention(this RewriterForContext<SignatureContext> rewriterFor)
        {
            InternalRewriterHelper.DeleteCallingConvention(rewriterFor.Rewriter, rewriterFor.Context.callingconvention());
            return rewriterFor;
        }

    }
}
