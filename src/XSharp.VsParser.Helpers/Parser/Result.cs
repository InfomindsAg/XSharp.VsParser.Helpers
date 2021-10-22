using System;
using System.Collections.Generic;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// The result class for the parse methods
    /// </summary>
    public class Result
    {
        /// <summary>
        /// An error or warning item
        /// </summary>
        public class Item
        {
            /// <summary>
            /// The message
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// The source code line number for the message
            /// </summary>
            public int Line { get; set; }

            /// <summary>
            /// The source code position for the message
            /// </summary>
            public int Position { get; set; }

            /// <summary>
            /// Converts the Item to a stirng
            /// </summary>
            /// <returns></returns>
            public override string ToString()
                => $"{Message} - Postion: {Position}";
        }

        /// <summary>
        /// The list of errors
        /// </summary>
        public List<Item> Errors { get; } = new();

        /// <summary>
        /// The list of warnings
        /// </summary>
        public List<Item> Warnings { get; } = new();

        /// <summary>
        /// OK is true, if the no errors were found
        /// </summary>
        public bool OK => Errors.Count == 0;

        /// <summary>
        /// Clears the result
        /// </summary>
        public void Clear()
        {
            Errors.Clear();
            Warnings.Clear();
        }
    }
}
