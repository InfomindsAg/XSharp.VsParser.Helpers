using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Classvars instance
    /// </summary>
    public class ClassvarsContextValues : ContextValues<ClassvarsContext>
    {
        /// <summary>
        /// The modifiers
        /// </summary>
        public string[] Modifiers { get; private set; }

        /// <summary>
        /// The list of variables
        /// </summary>
        public string[] Names { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; private set; }

        static internal ClassvarsContextValues Build(ClassvarsContext context)
        {
            if (context == null)
                return null;

            var variables = context.AsEnumerable().WhereType<LocalvarContext>().ToValues().ToArray();
            var isGroupType = !string.IsNullOrEmpty(variables.LastOrDefault()?.Type);
            if (variables.Length > 1)
                isGroupType = isGroupType && variables.Take(variables.Length - 1).All(q => string.IsNullOrEmpty(q.Type));

            return new ClassvarsContextValues
            {
                Context = context,
                //Names = context.Vars?._Var?.Select(q => q.Id.GetText()).ToArray(),
                //Type = context.Vars?.DataType?.GetText(),
                Modifiers = (context.Modifiers?._Tokens?.Select(q => q.Text) ?? Enumerable.Empty<string>()).ToArray(),
            };
        }
    }
}
