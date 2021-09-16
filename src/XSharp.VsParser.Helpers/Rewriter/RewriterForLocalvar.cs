using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForIdentifier Extensions
    /// </summary>
    public static class RewriterForLocalvar
    {
        /// <summary>
        /// Replaces the data type of the local variables with a new data type
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newDatatype">the new data type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<LocalvarContext> ReplaceDataType(this RewriterForContext<LocalvarContext> rewriterFor, string newDatatype)
        {
            if (string.IsNullOrEmpty(newDatatype))
                throw new ArgumentException("NewDataType can not be emtpy");

            var localContext = rewriterFor.Context;
            if (localContext.DataType != null)
                rewriterFor.Rewriter.Replace(localContext.datatype().Start.ToIndex(), localContext.datatype().stop.ToIndex(), newDatatype);
            else
            {
                var index = localContext.Id.stop.ToIndex();
                if (localContext.Expression != null)
                    index = localContext.Expression.Stop.ToIndex();
                rewriterFor.Rewriter.InsertAfter(index, $" as {newDatatype}");
            }

            return rewriterFor;
        }

    }
}
