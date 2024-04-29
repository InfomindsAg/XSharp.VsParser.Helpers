using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Parser.Values;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForMethodCall Extensions
    /// </summary>
    public static class RewriterForConstructorchainExtensions
    {
        /// <summary>
        /// Deletes all the arguments from the constructor super call
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorchainContext> DeleteAllArguments(this RewriterForContext<ConstructorchainContext> rewriterFor)
        {
            var argList = rewriterFor.Context.ArgList;
            if (MethodCallContextValues.IsArgListEmpty(argList))
                return rewriterFor;

            rewriterFor.Rewriter.Delete(argList.Start.ToIndex(), argList.Stop.ToIndex());
            return rewriterFor;
        }

        /// <summary>
        /// Replaces all the arguments from the constructor super call with new arguments
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newArguments">The new arguments</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ConstructorchainContext> ReplaceAllArguments(this RewriterForContext<ConstructorchainContext> rewriterFor, string newArguments)
        {
            var argList = rewriterFor.Context.ArgList;
            if (!MethodCallContextValues.IsArgListEmpty(argList))
                rewriterFor.Rewriter.Replace(argList.Start.ToIndex(), argList.Stop.ToIndex(), newArguments);
            else
                rewriterFor.Rewriter.Replace(rewriterFor.Context.LPAREN().ToIndex(), rewriterFor.Context.RPAREN().ToIndex(), $"({newArguments})");
            return rewriterFor;
        }
    }
}
