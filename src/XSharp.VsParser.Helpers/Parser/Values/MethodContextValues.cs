using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a MethodContext instance
    /// </summary>
    public class MethodContextValues : ContextValues<MethodContext>, ISignatureContextValues
    {
        /// <summary>
        /// The method signature values
        /// </summary>
        public SignatureContextValues Signature { get; internal set; }

        /// <summary>
        /// The method name
        /// </summary>
        public string Name => Signature.Name;

        /// <summary>
        /// The return tyoe
        /// </summary>
        public string ReturnType => Signature.ReturnType;

        /// <summary>
        /// The CallingConvention
        /// </summary>
        public string CallingConvention => Signature.CallingConvention;

        /// <summary>
        /// An array with the parameter values
        /// </summary>
        public ParameterContextValues[] Parameters => Signature.Parameters;

        static internal MethodContextValues Build(MethodContext context)
        {
            if (context == null)
                return null;

            return new MethodContextValues
            {
                Context = context,
                Signature = SignatureContextValues.Build(context.signature())
            };
        }
    }
}
