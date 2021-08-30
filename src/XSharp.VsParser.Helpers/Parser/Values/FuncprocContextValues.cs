using System.Linq;
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
    public class FuncprocContextValues : ContextValues<FuncprocContext>
    {
        /// <summary>
        /// The type of the Context (Function/Procedure)
        /// </summary>
        public ProcFuncType ProcFuncType { get; set; }

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

        static internal FuncprocContextValues Build(FuncprocContext context)
        {
            if (context == null)
                return null;

            var defType = (context.funcproctype().Any(q => q.PROCEDURE() != null)) ? ProcFuncType.Procedure : ProcFuncType.Function;

            var signature = context.signature();
            return new FuncprocContextValues
            {
                ProcFuncType = defType,
                Context = context,
                Name = signature.identifier()?.GetText(),
                CallingConvention = signature.CallingConvention?.GetText(),
                ReturnType = signature.Type?.GetText(),
                Parameters = (signature.parameterList()?.AsEnumerable().WhereType<ParameterContext>().ToValues() ?? Enumerable.Empty<ParameterContextValues>()).ToArray(),
            };
        }
    }
}
