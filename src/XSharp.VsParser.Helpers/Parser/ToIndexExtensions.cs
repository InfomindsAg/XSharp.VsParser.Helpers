using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static class ToIndexExtensions
    {
        public static int ToIndex(this IToken token)
        {
            if (token is XSharpToken xsToken && xsToken != null)
                return xsToken.OriginalTokenIndex;

            throw new ArgumentException("Token must be a XSharpToken and can not be null");
        }

        public static int ToIndex(this IdentifierContext id)
            => ToIndex(id?.Token);
    }
}
