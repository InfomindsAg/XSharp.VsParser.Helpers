using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a NamedArgumentContext instance
    /// </summary>
    public class NamedArgumentContextValues : ContextValues<NamedArgumentContext>
    {
        /// <summary>
        /// The argument name (only for named argument)
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// The argument value
        /// </summary>
        public string Value { get; internal set; }

        static internal NamedArgumentContextValues Build(NamedArgumentContext context)
        {
            if (context == null)
                return null;

            var result = new NamedArgumentContextValues { Context = context, Name = context.Name?.GetText() };

            if (!string.IsNullOrEmpty(result.Name))
                result.Value = context.Expr?.GetText();
            else
                result.Value = context.GetText();

            return result;
        }

        static internal bool IsEmpty(NamedArgumentContext context)
        {
            return context == null || (context.Name == null && context.Expr == null);
        }
    }
}
