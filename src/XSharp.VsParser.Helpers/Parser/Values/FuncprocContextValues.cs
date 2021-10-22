using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Type of the FuncprocContext
    /// </summary>
    public enum ProcFuncType
    {
        /// <summary>
        /// Context is a Function
        /// </summary>
        Function = 1,

        /// <summary>
        /// Context is a Procedure
        /// </summary>
        Procedure = 2
    }


    /// <summary>
    /// Values class for a FuncprocContext instance
    /// </summary>
    public class FuncprocContextValues : ContextValues<FuncprocContext>, ISignatureContextValues
    {
        /// <summary>
        /// The type of the Context (Function/Procedure)
        /// </summary>
        public ProcFuncType ProcFuncType { get; internal set; }

        /// <summary>
        /// The type of the Context (Function/Procedure)
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

        static internal FuncprocContextValues Build(FuncprocContext context)
        {
            if (context == null)
                return null;

            var defType = (context.funcproctype().Any(q => q.PROCEDURE() != null)) ? ProcFuncType.Procedure : ProcFuncType.Function;

            return new FuncprocContextValues
            {
                ProcFuncType = defType,
                Context = context,
                Signature = SignatureContextValues.Build(context.signature())
            };
        }
    }
}
