using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a ParameterContext instance
    /// </summary>
    public class ParameterContextValues : ContextValues<ParameterContext>
    {
        /// <summary>
        /// The parameter name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The data type of the parameter
        /// </summary>
        public string DataType { get; internal set; }

        /// <summary>
        /// The default value of the parameter
        /// </summary>
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

        static internal bool IsEmpty(ParameterContext context)
        {
            return context == null || string.IsNullOrWhiteSpace(context.identifier()?.GetText());
        }
    }
}
