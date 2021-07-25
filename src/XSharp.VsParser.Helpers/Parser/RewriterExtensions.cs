using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static class RewriterExtensions
    {
        public static void ReplaceIdentifier(this TokenStreamRewriter rewriter, IdentifierContext identifier, string newIdentifier)
        {
            rewriter.Replace(identifier.ToIndex(), newIdentifier);
        }

        public static void ReplaceCallingConvention(this TokenStreamRewriter rewriter, SignatureContext signature, string newCallingConvention)
        {
            var callingConvention = signature.callingconvention();
            if (callingConvention != null)
                rewriter.Replace(callingConvention.Convention.ToIndex(), newCallingConvention);
            else
                rewriter.InsertAfter(signature.Stop.ToIndex(), " " + newCallingConvention);
        }

    }
}
