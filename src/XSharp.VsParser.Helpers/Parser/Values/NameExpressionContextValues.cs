using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a NameExpressionContext instance
    /// </summary>
    public class NameExpressionContextValues : ContextValues<NameExpressionContext>
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; internal set; }

        static internal NameExpressionContextValues Build(NameExpressionContext context)
        {
            if (context == null)
                return null;
            
            return new NameExpressionContextValues
            {
                Context = context,
                Name = context.simpleName()?.Id.GetText(),
            };
        }
    }
}