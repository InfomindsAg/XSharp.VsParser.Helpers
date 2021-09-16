using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Localvar instance
    /// </summary>
    public class LocalvarContextValues : ContextValues<LocalvarContext>
    {
        /// <summary>
        /// The method name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The return type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The initialisation expression
        /// </summary>
        public string InitExpression { get; private set; }

        static internal LocalvarContextValues Build(LocalvarContext context)
        {
            if (context == null)
                return null;

            return new LocalvarContextValues
            {
                Context = context,
                Name = context.Id.GetText(),
                Type = context.DataType?.GetText(),
                InitExpression = context.Expression?.GetText()
            };
        }
    }
}
