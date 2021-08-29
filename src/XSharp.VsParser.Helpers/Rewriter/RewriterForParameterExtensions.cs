using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Rewriter;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    public static class RewriterForParameterExtensions
    {

        #region SuperExpression

        public static RewriterForContext<ParameterContext> ReplaceParameterDataType(this RewriterForContext<ParameterContext> rewriterFor, string datatype)
        {
            var parameterContext = rewriterFor.Context;
            if (parameterContext.datatype() != null)
                rewriterFor.Rewriter.Replace(parameterContext.datatype().Start.ToIndex(), parameterContext.datatype().stop.ToIndex(), datatype);
            else
            {
                var index = parameterContext.identifier().ToIndex();
                if (parameterContext.Default != null)
                    index = parameterContext.Default.stop.ToIndex();
                rewriterFor.Rewriter.InsertAfter(index, $" as {datatype}");
            }

            return rewriterFor;
        }

        #endregion

    }
}
