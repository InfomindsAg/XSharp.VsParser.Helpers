using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// ToIndex extensions
    /// </summary>
    public static class ToIndexExtensions
    {
        /// <summary>
        /// Returns the OriginalTokenIndex for a token
        /// </summary>
        /// <param name="token">A token</param>
        /// <returns>The OriginalTokenIndex</returns>
        public static int ToIndex(this IToken token)
        {
            if (token is XSharpToken xsToken && xsToken != null)
                return xsToken.OriginalTokenIndex;

            throw new ArgumentException("Token must be a XSharpToken and can not be null");
        }

        /// <summary>
        /// Returns the OriginalTokenIndex for an IdentifierContext
        /// </summary>
        /// <param name="id">An IdentifierContext</param>
        /// <returns>The OriginalTokenIndex</returns>
        public static int ToIndex(this IdentifierContext id)
            => ToIndex(id?.Token);
    }
}
