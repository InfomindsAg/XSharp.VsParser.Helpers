using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// Comment
    /// </summary>
    public class Comment
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
        public int StartLine { get; set; }
        /// <summary>
        /// The line where the comment ends (1-based)
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// The column in the line where the comment starts (1-based)
        /// </summary>
        public int StartColumn { get; set; }

        /// <summary>
        /// The column in the line where the comment ends (1-based)
        /// </summary>
        public int EndColumn { get; set; }

        static internal Comment Build(IToken context, ParserHelper parserHelper)
        {
            if (context == null)
                return null;

            var pos = parserHelper.GetTokenPosition(context);

            return new Comment
            {
                Context = context,
                Text = context.Text,
                StartLine = pos.StartLine,
                StartColumn = pos.StartColumn,
                EndLine = pos.EndLine,
                EndColumn = pos.EndColumn,
            };
        }
    }
}
