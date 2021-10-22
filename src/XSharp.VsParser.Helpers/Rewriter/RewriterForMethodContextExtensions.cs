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
        /// Deletes all the parameters of the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteAllParameters(this RewriterForContext<MethodContext> rewriterFor)
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
        public static RewriterForContext<MethodContext> ReplaceParameters(this RewriterForContext<MethodContext> rewriterFor, string newParameters)
        {
            RewriterForSignature(rewriterFor).ReplaceParameters(newParameters);
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
        /// Adds a modifier to the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="modifiers">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> AddModifiers(this RewriterForContext<MethodContext> rewriterFor, string modifiers)
        {
            if (string.IsNullOrEmpty(modifiers))
                throw new ArgumentException("Modifier can not be emtpy");

            foreach (var modifier in modifiers.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                rewriterFor.DeleteModifier(modifier);

            rewriterFor.Rewriter.InsertBefore(rewriterFor.Context.methodtype(0).Token.ToIndex(), modifiers + " ");

            return rewriterFor;
        }

        /// <summary>
        /// Deletes the specified modifier for the method
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="modifier">The modifier, that should be deleted</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteModifier(this RewriterForContext<MethodContext> rewriterFor, string modifier)
        {
            var currentModifiers = rewriterFor.Context.Modifiers;
            if (currentModifiers != null)
            {
                var token = currentModifiers._Tokens?.FirstOrDefault(q => modifier.Equals(q.Text, StringComparison.OrdinalIgnoreCase));
                if (token != null)
                {
                    
                    if (token == currentModifiers._Tokens.Last())
                        rewriterFor.Rewriter.Delete(token.ToIndex(), rewriterFor.Context.methodtype(0).Start.ToIndex() - 1);
                    else
                    {
                        var tokenIndex = currentModifiers._Tokens.IndexOf(token);
                        rewriterFor.Rewriter.Delete(token.ToIndex(), currentModifiers._Tokens[tokenIndex + 1].ToIndex() - 1);
                    }
                }
            }
            return rewriterFor;
        }

        /// <summary>
        /// Deletes all the method modifiers
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<MethodContext> DeleteAllModifiers(this RewriterForContext<MethodContext> rewriterFor)
        {
            var currentModifiers = rewriterFor.Context.Modifiers;
            if (currentModifiers != null && currentModifiers._Tokens?.Any() == true)
                rewriterFor.Rewriter.Delete(currentModifiers.Start.ToIndex(), rewriterFor.Context.methodtype(0).Start.ToIndex() - 1);
            return rewriterFor;
        }
    }
}
