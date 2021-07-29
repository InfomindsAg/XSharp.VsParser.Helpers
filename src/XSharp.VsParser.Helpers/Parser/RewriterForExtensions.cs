using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static class RewriterForExtensions
    {
        #region Identifier 

        public static RewriterForContext<IdentifierContext> ReplaceIdentifier(this RewriterForContext<IdentifierContext> rewriterFor, string newIdentifier)
        {
            rewriterFor.Rewriter.Replace(rewriterFor.Context.ToIndex(), newIdentifier);
            return rewriterFor;
        }

        #endregion

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
            var returnContext = rewriterFor.Context.Type;
            if (string.IsNullOrEmpty(newReturnType))
            {
                if (returnContext != null)
                    rewriterFor.Rewriter.Delete(returnContext.start.ToIndex(), returnContext.stop.ToIndex());
                return rewriterFor;
            }

            if (!newReturnType.TrimStart().StartsWith("as", StringComparison.OrdinalIgnoreCase))
                newReturnType = " as " + newReturnType;

            if (returnContext != null)
                rewriterFor.Rewriter.Replace(returnContext.start.ToIndex(), returnContext.stop.ToIndex(), newReturnType);
            else
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.ParamList.Stop.ToIndex(), newReturnType);
            return rewriterFor;
        }

        public static RewriterForContext<SignatureContext> ReplaceCallingConvention(this RewriterForContext<SignatureContext> rewriterFor, string newCallingConvention)
        {
            var callingConvention = rewriterFor.Context.callingconvention();
            if (callingConvention != null)
                rewriterFor.Rewriter.Replace(callingConvention.Convention.ToIndex(), newCallingConvention);
            else
                rewriterFor.Rewriter.InsertAfter(rewriterFor.Context.Stop.ToIndex(), " " + newCallingConvention);
            return rewriterFor;
        }

        #endregion

        #region MethodContext

        static RewriterForContext<SignatureContext> RewriterForSignature(RewriterForContext<MethodContext> rewriterFor)
            => rewriterFor.RewriterFor(rewriterFor.Context.Sig);

        public static RewriterForContext<MethodContext> DeleteAllParameters(this RewriterForContext<MethodContext> rewriterFor)
        {
            rewriterFor.DeleteAllParameters();
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

        public static RewriterForContext<MethodContext> ReplaceCallingConvention(this RewriterForContext<MethodContext> rewriterFor, string newCallingConvention)
        {
            RewriterForSignature(rewriterFor).ReplaceCallingConvention(newCallingConvention);
            return rewriterFor;
        }


        #endregion

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

        #region MethodCall

        public static RewriterForContext<MethodCallContext> DeleteAllArguments(this RewriterForContext<MethodCallContext> rewriterFor)
        {
            var argList = rewriterFor.Context.ArgList;
            if ((argList?.ChildCount ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Delete(argList.Start.ToIndex(), argList.Stop.ToIndex());
            return rewriterFor;
        }

        #endregion

        #region ReturnStmt

        public static RewriterForContext<ReturnStmtContext> DeleteExpression(this RewriterForContext<ReturnStmtContext> rewriterFor)
        {
            var expr = rewriterFor.Context.Expr;
            if ((expr?.ChildCount ?? 0) == 0)
                return rewriterFor;

            rewriterFor.Rewriter.Delete(expr.Start.ToIndex(), expr.Stop.ToIndex());
            return rewriterFor;
        }

        #endregion

    }
}
