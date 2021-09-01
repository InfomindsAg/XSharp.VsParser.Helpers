using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a MethodContext instance
    /// </summary>
    public class ConstructorContextValues : ContextValues<ConstructorContext>
    {
        /// <summary>
        /// The CallingConvention
        /// </summary>
        public string CallingConvention { get; internal set; }

        /// <summary>
        /// An array with the parameter values
        /// </summary>
        public ParameterContextValues[] Parameters { get; internal set; }

        static internal ConstructorContextValues Build(ConstructorContext context)
        {
            if (context == null)
                return null;

            return new ConstructorContextValues
            {
                Context = context,
                CallingConvention = context.CallingConvention?.GetText(),
                Parameters = (context.parameterList()?.AsEnumerable().WhereType<ParameterContext>().ToValues() ?? Enumerable.Empty<ParameterContextValues>()).ToArray(),
            };
        }
    }
}
