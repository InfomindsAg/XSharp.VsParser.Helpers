using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Type of the FuncprocContext
    /// </summary>
    public enum MethodType
    {
        /// <summary>
        /// Context is a Method
        /// </summary>
        Method = 1,

        /// <summary>
        /// Context is a Access
        /// </summary>
        Access = 2,

        /// <summary>
        /// Context is a Assign
        /// </summary>
        Assign = 3
    }

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
        /// The type
        /// </summary>
        public MethodType MethodType { get; private set; }

        /// <summary>
        /// The modifiers
        /// </summary>
        public string[] Modifiers { get; private set; }

        /// <summary>
        /// The method name
        /// </summary>
        public string Name => Signature.Name;

        /// <summary>
        /// The return type
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

            var methodType = MethodType.Method;
            if (context.methodtype().Any(q => q.ASSIGN() != null))
                methodType = MethodType.Assign;
            else if (context.methodtype().Any(q => q.ACCESS() != null))
                methodType = MethodType.Access;

            return new MethodContextValues
            {
                Context = context,
                MethodType = methodType,
                Modifiers = (context.Modifiers?._Tokens?.Select(q => q.Text) ?? Enumerable.Empty<string>()).ToArray(),
                Signature = SignatureContextValues.Build(context.signature())
            };
        }
    }
}
