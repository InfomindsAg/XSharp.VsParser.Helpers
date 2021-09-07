using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    static class InternalRewriterHelper
    {

        public static void ReplaceCallingConvention(TokenStreamRewriter rewriter, string newCallingConvention, CallingconventionContext callingConvention, IToken insertAfterToken)
        {
            if (string.IsNullOrEmpty(newCallingConvention))
                throw new ArgumentException($"{nameof(newCallingConvention)} can not be empty");

            if (callingConvention != null)
                rewriter.Replace(callingConvention.Convention.ToIndex(), newCallingConvention);
            else
            {
                if (insertAfterToken == null)
                    throw new ArgumentException($"{nameof(insertAfterToken)} can not be null");
                rewriter.InsertAfter(insertAfterToken.ToIndex(), " " + newCallingConvention);
            }
        }

        public static void DeleteCallingConvention(TokenStreamRewriter rewriter, CallingconventionContext callingConvention)
        {
            if (callingConvention != null)
                rewriter.Delete(callingConvention.Convention.ToIndex());
        }
    }
}
