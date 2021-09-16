using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Localvar instance
    /// </summary>
    public class CommonLocalDeclContextValues : ContextValues<CommonLocalDeclContext>
    {
        /// <summary>
        /// The list of variables
        /// </summary>
        public LocalvarContextValues[] Variables { get; private set; }

        /// <summary>
        /// True, if all the variables get the type of the last variable
        /// </summary>
        public bool IsGroupType { get; private set; }

        static internal CommonLocalDeclContextValues Build(CommonLocalDeclContext context)
        {
            if (context == null)
                return null;

            var variables = context.AsEnumerable().WhereType<LocalvarContext>().ToValues().ToArray();
            var isGroupType = !string.IsNullOrEmpty(variables.LastOrDefault()?.Type);
            if (variables.Length > 1)
                isGroupType = isGroupType && variables.Take(variables.Length - 1).All(q => string.IsNullOrEmpty(q.Type));

            return new CommonLocalDeclContextValues
            {
                Context = context,
                Variables = variables,
                IsGroupType = isGroupType
            };
        }
    }
}
