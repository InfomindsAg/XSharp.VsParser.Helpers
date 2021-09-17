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

        static internal CommonLocalDeclContextValues Build(CommonLocalDeclContext context)
        {
            if (context == null)
                return null;

            var variables = context.AsEnumerable().WhereType<LocalvarContext>().ToValues().ToArray();
            if (variables.Length > 1)
            {
                string lastType = null;
                foreach (var item in variables.Reverse())
                {
                    if (!string.IsNullOrEmpty(item.Type))
                        lastType = item.Type;
                    else
                        item.Type = lastType ;
                }
            }

            return new CommonLocalDeclContextValues
            {
                Context = context,
                Variables = variables,
            };
        }
    }
}
