using System;
using System.Collections.Generic;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// The type of a Token
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The token is a keyword
        /// </summary>
        Keyword = 1,

        /// <summary>
        /// The token is a positional keyword
        /// </summary>
        PositionalKeyword = 2,

        /// <summary>
        /// The token is a modifier
        /// </summary>
        Modifier = 3,

        /// <summary>
        /// The token is a identifier
        /// </summary>
        Identifier = 4,

        /// <summary>
        /// The token is a type
        /// </summary>
        Type = 5,

        /// <summary>
        /// The token is a string
        /// </summary>
        String = 6,

        /// <summary>
        /// The token is a constant
        /// </summary>
        Constant = 7,

        /// <summary>
        /// The token is a comment
        /// </summary>
        Comment = 8,

        /// <summary>
        /// The Token is something else
        /// </summary>
        Other = 0,
    }
}
