using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForPropertyClassVar Extensions
    /// </summary>
    public static class RewriterForClassvarContextExtensions
    {

        /// <summary>
        /// Replaces the type of the class var
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newType">The new type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ClassvarContext> ReplaceType(this RewriterForContext<ClassvarContext> rewriterFor, string newType)
        {
            if (string.IsNullOrEmpty(newType))
                throw new ArgumentException($"{nameof(newType)} can not be empty");

            var context = rewriterFor.Context;
            var typeContext = context.DataType;
            if (typeContext != null && HasTypeDefinition(context))
            {
                rewriterFor.Rewriter.Replace(typeContext.start.ToIndex(), typeContext.stop.ToIndex(), InternalRewriterHelper.RemoveAsFromType(newType));
            }
            else
                rewriterFor.Rewriter.InsertAfter(context.stop.ToIndex(), " " + InternalRewriterHelper.AddAsToType(newType));

            MoveTypeDefinition(rewriterFor, context);

            return rewriterFor;
        }

        private static void MoveTypeDefinition(RewriterForContext<ClassvarContext> rewriterFor, ClassvarContext variable)
        {
            var prevVariable = GetPreviousVariable(variable);
            if (prevVariable != null && !HasTypeDefinition(prevVariable))
            {
                rewriterFor.Rewriter.InsertAfter(prevVariable.stop.ToIndex(), " " + InternalRewriterHelper.AddAsToType(variable.DataType.GetText()));
            }
        }
        private static bool HasTypeDefinition(ClassvarContext classvar) => classvar.As != null;

        private static ClassvarContext GetPreviousVariable(ClassvarContext context)
        {
            var parent = context.Parent as ClassvarsContext;

            return parent.children.WhereType<ClassvarContext>()
                .TakeWhile(x => x != context)
                .LastOrDefault();
        }
    }
}
