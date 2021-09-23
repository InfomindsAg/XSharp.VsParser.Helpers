using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// Comment
    /// </summary>
    public class TokenValues
    {
        /// <summary>
        /// The Context, from which the values were extracted
        /// </summary>
        public IToken Context { get; internal set; }

        /// <summary>
        /// The comment text
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// The line where the comment starts (1-based)
        /// </summary>
        public int StartLine { get; internal set; }
        /// <summary>
        /// The line where the comment ends (1-based)
        /// </summary>
        public int EndLine { get; internal set; }

        /// <summary>
        /// The column in the line where the comment starts (1-based)
        /// </summary>
        public int StartColumn { get; internal set; }

        /// <summary>
        /// The column in the line where the comment ends (1-based)
        /// </summary>
        public int EndColumn { get; internal set; }

        /// <summary>
        /// The Token Type
        /// </summary>
        public TokenType Type { get; internal set; }
    }
}
