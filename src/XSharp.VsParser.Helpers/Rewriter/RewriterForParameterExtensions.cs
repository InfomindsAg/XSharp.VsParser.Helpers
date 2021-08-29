using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    /// <summary>
    /// RewriterForParameter Extensions
    /// </summary>
    public static class RewriterForParameterExtensions
    {

        /// <summary>
        /// Replaces the name of the parameter with a new name
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newName">the new name</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ParameterContext> ReplaceParameterName(this RewriterForContext<ParameterContext> rewriterFor, string newName)
        {
            var parameterContext = rewriterFor.Context;
            if (parameterContext.ToValues().Name != newName)
                rewriterFor.RewriterFor(parameterContext.Id).ReplaceIdentifier(newName);

            return rewriterFor;
        }


        /// <summary>
        /// Replaces the data type of the parameter with a new data type
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newDatatype">the new data type</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ParameterContext> ReplaceParameterDataType(this RewriterForContext<ParameterContext> rewriterFor, string newDatatype)
        {
            var parameterContext = rewriterFor.Context;
            if (parameterContext.datatype() != null)
                rewriterFor.Rewriter.Replace(parameterContext.datatype().Start.ToIndex(), parameterContext.datatype().stop.ToIndex(), newDatatype);
            else
            {
                var index = parameterContext.identifier().ToIndex();
                if (parameterContext.Default != null)
                    index = parameterContext.Default.stop.ToIndex();
                rewriterFor.Rewriter.InsertAfter(index, $" as {newDatatype}");
            }

            return rewriterFor;
        }

    }
}
