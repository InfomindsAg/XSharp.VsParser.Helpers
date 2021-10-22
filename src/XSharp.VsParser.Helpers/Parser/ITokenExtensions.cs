using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// Extensions for IToken
    /// </summary>
    public static class ITokenExtensions
    {
        /// <summary>
        ///  Gets the Type of the Token
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>The type</returns>
        public static TokenType GetTokenType(this IToken token)
        {
            if (XSharpLexer.IsComment(token.Type))
                return TokenType.Comment;
            if (XSharpLexer.IsConstant(token.Type))
                return TokenType.Constant;
            if (XSharpLexer.IsIdentifier(token.Type))
                return TokenType.Identifier;
            if (XSharpLexer.IsKeyword(token.Type))
                return TokenType.Keyword;
            if (XSharpLexer.IsPositionalKeyword(token.Type))
                return TokenType.PositionalKeyword;
            if (XSharpLexer.IsModifier(token.Type))
                return TokenType.Modifier;
            if (XSharpLexer.IsString(token.Type))
                return TokenType.String;
            if (XSharpLexer.IsType(token.Type))
                return TokenType.Type;

            return TokenType.Other;
        }

    }
}
