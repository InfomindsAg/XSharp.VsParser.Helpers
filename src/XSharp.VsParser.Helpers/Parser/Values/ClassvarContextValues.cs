using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Classvar instance
    /// </summary>
    public class ClassvarContextValues : ContextValues<ClassvarContext>
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The initialisation expression
        /// </summary>
        public string InitExpression { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; private set; }

        static internal ClassvarContextValues Build(ClassvarContext context)
        {
            if (context == null)
                return null;

            return new ClassvarContextValues
            {
                Context = context,
                Name = context.Id.GetText(),
                InitExpression = context.expression()?.GetText(),
                Type = context.DataType?.GetText(),
            };
        }
    }
}
