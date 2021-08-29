using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForIdentifier Extensions
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
        /// Delates all the parameters of the signature
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> DeleteAllParameters(this RewriterForContext<SignatureContext> rewriterFor)
        {
            var paramList = rewriterFor.Context?.ParamList;
            if ((paramList?._Params?.Count ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Replace(paramList.Start.ToIndex(), paramList.Stop.ToIndex(), "()");
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
            {
                if (newReturnType.TrimStart().StartsWith("as ", StringComparison.OrdinalIgnoreCase))
                    newReturnType = newReturnType.TrimStart().Substring(3).TrimStart();
                rewriterFor.Rewriter.Replace(returnContext.start.ToIndex(), returnContext.stop.ToIndex(), newReturnType);
            }
            else
            {
                if (!newReturnType.TrimStart().StartsWith("as ", StringComparison.OrdinalIgnoreCase))
                    newReturnType = " as " + newReturnType;
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.ParamList.Stop.ToIndex(), newReturnType);
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
            if (string.IsNullOrEmpty(newCallingConvention))
                throw new ArgumentException($"{nameof(newCallingConvention)} can not be empty");

            var callingConvention = rewriterFor.Context.callingconvention();
            if (callingConvention != null)
                rewriterFor.Rewriter.Replace(callingConvention.Convention.ToIndex(), newCallingConvention);
            else
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.Stop.ToIndex(), " " + newCallingConvention);
            return rewriterFor;
        }

        /// <summary>
        /// Deletes the calling convention
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<SignatureContext> DeleteCallingConvention(this RewriterForContext<SignatureContext> rewriterFor)
        {
            var callingConvention = rewriterFor.Context.callingconvention();
            if (callingConvention != null)
                rewriterFor.Rewriter.Delete(callingConvention.Convention.ToIndex());
            return rewriterFor;
        }

    }
}
