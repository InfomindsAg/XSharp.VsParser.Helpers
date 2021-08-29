using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    public class ParameterContextValues : ContextValues<ParameterContext>
    {
        public string Name { get; internal set; }
        public string DataType { get; internal set; }
        public string Default { get; internal set; }

        static internal ParameterContextValues Build(ParameterContext context)
        {
            if (context == null)
                return null;

            return new ParameterContextValues
            {
                Context = context,
                Name = context.identifier()?.GetText(),
                DataType = context.datatype()?.GetText(),
                Default = context.Default?.GetText(),
            };
        }
    }
}
