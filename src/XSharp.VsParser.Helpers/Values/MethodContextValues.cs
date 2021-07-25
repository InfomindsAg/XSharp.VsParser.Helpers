using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Values
{
    public class MethodContextValues
    {
        // TODO: Extend with Parameters, Return-Type, Calling Convention, Parameters

        public string Name { get; internal set; }

        static internal MethodContextValues Build(MethodContext context)
        {
            if (context == null)
                return null;

            var signature = context.signature();
            return new MethodContextValues
            {
                Name = signature.identifier()?.GetText(),
            };
        }
    }
}
