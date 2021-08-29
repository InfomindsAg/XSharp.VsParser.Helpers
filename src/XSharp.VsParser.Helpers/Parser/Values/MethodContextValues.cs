using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    public class MethodContextValues : ContextValues<MethodContext>
    {
        public string Name { get; internal set; }
        public string ReturnType { get; internal set; }
        public string CallingConvetion { get; internal set; }

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
                CallingConvetion = signature.CallingConvention?.GetText(),
                ReturnType = signature.Type?.GetText(),
                Parameters = (signature.parameterList()?.AsEnumerable().WhereType<ParameterContext>().ToValues() ?? Enumerable.Empty<ParameterContextValues>()).ToArray() ,
            };
        }
    }
}
