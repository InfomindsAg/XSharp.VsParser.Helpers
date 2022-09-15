using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
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
        {
            if (id == null)
                throw new ArgumentException("id can not be null");
            else if (id.ID() != null)
                return ToIndex(id.ID());
            else if (id.keywordxs()?.Token != null)
                return ToIndex(id.keywordxs()?.Token);

            throw new ArgumentException("Can not convert ID to Index");
        }

        /// <summary>
        /// Returns the OriginalTokenIndex for a token
        /// </summary>
        /// <param name="terminalNode">A terminalNode</param>
        /// <returns>The OriginalTokenIndex</returns>
        public static int ToIndex(this ITerminalNode terminalNode)
        {
            if (terminalNode?.Symbol != null)
                return terminalNode.Symbol.ToIndex();

            throw new ArgumentException("TerminalNode can not be null");
        }
    }
}
