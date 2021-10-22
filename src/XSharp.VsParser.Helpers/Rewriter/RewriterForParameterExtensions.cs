using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
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
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("NewName can not be emtpy");

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
            if (string.IsNullOrEmpty(newDatatype))
                throw new ArgumentException("NewDataType can not be emtpy");

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

        /// <summary>
        /// Replaces the default value of the parameter with a new default value
        /// </summary>
        /// <param name="rewriterFor">The rewriterFor instance</param>
        /// <param name="newDefault">the new default value</param>
        /// <returns>The rewriterFor instance</returns>
        public static RewriterForContext<ParameterContext> ReplaceParameterDefaultValue(this RewriterForContext<ParameterContext> rewriterFor, string newDefault)
        {
            if (string.IsNullOrEmpty(newDefault))
                throw new ArgumentException("newDefault can not be emtpy");

            var parameterContext = rewriterFor.Context;
            if (parameterContext.Default != null)
                rewriterFor.Rewriter.Replace(parameterContext.Default.Start.ToIndex(), parameterContext.Default.stop.ToIndex(), newDefault);
            else
                rewriterFor.Rewriter.InsertAfter(parameterContext.identifier().ToIndex(), $" := {newDefault}");

            return rewriterFor;
        }

    }
}
