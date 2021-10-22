using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a NamedArgumentContext instance
    /// </summary>
    public class NamedArgumentContextValues : ContextValues<NamedArgumentContext>
    {
        /// <summary>
        /// The argument value
        /// </summary>
        public string Value { get; internal set; }

        static internal NamedArgumentContextValues Build(NamedArgumentContext context)
        {
            if (context == null)
                return null;

            return new NamedArgumentContextValues
            {
                Context = context,
                Value = context.GetText(),
            };
        }
    }
}
