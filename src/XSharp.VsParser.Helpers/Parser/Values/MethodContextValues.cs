using System.Linq;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a MethodContext instance
    /// </summary>
    public class MethodContextValues : ContextValues<MethodContext>
    {
        /// <summary>
        /// The method name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The return tyoe
        /// </summary>
        public string ReturnType { get; internal set; }

        /// <summary>
        /// The CallingConvention
        /// </summary>
        public string CallingConvention { get; internal set; }

        /// <summary>
        /// An array with the parameter values
        /// </summary>
        public ParameterContextValues[] Parameters { get; internal set; }

        static internal MethodContextValues Build(MethodContext context)
        {
            if (context == null)
                return null;

            var signature = context.signature();
            return new MethodContextValues
            {
                Context = context,
                Name = signature.identifier()?.GetText(),
                CallingConvention = signature.CallingConvention?.GetText(),
                ReturnType = signature.Type?.GetText(),
                Parameters = (signature.parameterList()?.AsEnumerable().WhereType<ParameterContext>().ToValues() ?? Enumerable.Empty<ParameterContextValues>()).ToArray() ,
            };
        }
    }
}
