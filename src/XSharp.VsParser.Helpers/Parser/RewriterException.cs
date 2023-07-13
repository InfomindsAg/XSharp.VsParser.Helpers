using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// An empty rewrite resulted in a code change 
    /// </summary>
    public class RewriterException : InvalidOperationException
    {
        /// <summary>
        /// Original Sourcecode
        /// </summary>
        public string OriginalCode { get; private set; }

        /// <summary>
        /// Sourcecode after empty rewrite
        /// </summary>
        public string EmptyRewriteCode { get; private set; }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="originalCode">Original Sourcecode</param>
        /// <param name="emptyRewriteCode">Sourcecode after empty rewrite</param>
        public RewriterException(string originalCode, string emptyRewriteCode) : this("Emtpy Rewriter created unexprected changes to the code!", originalCode, emptyRewriteCode)
        {
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="originalCode">Original Sourcecode</param>
        /// <param name="emptyRewriteCode">Sourcecode after empty rewrite</param>
        public RewriterException(string message, string originalCode, string emptyRewriteCode) : base(message)
        {
            OriginalCode = originalCode;
            EmptyRewriteCode = emptyRewriteCode;
        }

    }
}
