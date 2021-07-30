using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForMethodCallExtensions
    {

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

    }
}
