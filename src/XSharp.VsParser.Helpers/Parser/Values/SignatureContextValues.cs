using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a MethodContext instance
    /// </summary>
    public class SignatureContextValues : ContextValues<SignatureContext>, ISignatureContextValues
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

        static internal SignatureContextValues Build(SignatureContext context)
        {
            if (context == null)
                return null;

            return new SignatureContextValues
            {
                Context = context,
                Name = context.identifier()?.GetText(),
                CallingConvention = context.CallingConvention?.GetText(),
                ReturnType = context.Type?.GetText(),
                Parameters = (context.parameterList()?.AsEnumerable().WhereType<ParameterContext>().ToValues() ?? Enumerable.Empty<ParameterContextValues>()).ToArray(),
            };
        }
    }
}
