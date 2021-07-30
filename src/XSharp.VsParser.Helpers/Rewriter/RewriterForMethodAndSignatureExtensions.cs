using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForMethodAndSignatureExtensions
    {
        #region Signature

        public static RewriterForContext<SignatureContext> ReplaceMethodName(this RewriterForContext<SignatureContext> rewriterFor, string newMethodName)
        {
            new RewriterForContext<IdentifierContext>(rewriterFor.Rewriter, rewriterFor.Context.Id).ReplaceIdentifier(newMethodName);
            return rewriterFor;
        }

        public static RewriterForContext<SignatureContext> DeleteAllParameters(this RewriterForContext<SignatureContext> rewriterFor)
        {
            var paramList = rewriterFor.Context?.ParamList;
            if ((paramList?._Params?.Count ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Replace(paramList.Start.ToIndex(), paramList.Stop.ToIndex(), "()");
            return rewriterFor;
        }

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

        public static RewriterForContext<SignatureContext> DeleteCallingConvention(this RewriterForContext<SignatureContext> rewriterFor)
        {
            var callingConvention = rewriterFor.Context.callingconvention();
            if (callingConvention != null)
                rewriterFor.Rewriter.Delete(callingConvention.Convention.ToIndex());
            return rewriterFor;
        }


        #endregion

        #region MethodContext

        static RewriterForContext<SignatureContext> RewriterForSignature(RewriterForContext<MethodContext> rewriterFor)
            => rewriterFor.RewriterFor(rewriterFor.Context.Sig);

        public static RewriterForContext<MethodContext> DeleteAllParameters(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteAllParameters();
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> ReplaceMethodName(this RewriterForContext<MethodContext> rewriterFor, string newMethodName)
        {
            RewriterForSignature(rewriterFor).ReplaceMethodName(newMethodName);
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> ReplaceReturnType(this RewriterForContext<MethodContext> rewriterFor, string newReturnType)
        {
            RewriterForSignature(rewriterFor).ReplaceReturnType(newReturnType);
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> DeleteReturnType(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteReturnType();
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> ReplaceCallingConvention(this RewriterForContext<MethodContext> rewriterFor, string newCallingConvention)
        {
            RewriterForSignature(rewriterFor).ReplaceCallingConvention(newCallingConvention);
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> DeleteCallingConvention(this RewriterForContext<MethodContext> rewriterFor)
        {
            RewriterForSignature(rewriterFor).DeleteCallingConvention();
            return rewriterFor;
        }

        public static RewriterForContext<MethodContext> AddOverride(this RewriterForContext<MethodContext> rewriterFor)
        {
            var modifiers = rewriterFor.Context.Modifiers;
            if (modifiers == null || modifiers.OVERRIDE().Length == 0)
                rewriterFor.Rewriter.InsertBefore(rewriterFor.Context.methodtype(0).Token.ToIndex(), "override ");
            return rewriterFor;
        }

        #endregion

    }
}
