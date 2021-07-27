using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Values
{
    public class MethodContextValues : ContextValues<MethodContext>
    {
        // TODO: Extend with Parameters, Return-Type, Calling Convention, Parameters

        public string Name { get; internal set; }
        public string ReturnType { get; internal set; }
        public string CallingConvetion { get; internal set; }

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
            };
        }
    }
}
