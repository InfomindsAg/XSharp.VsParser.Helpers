using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Classvar instance
    /// </summary>
    public class ClassVarListContextValues : ContextValues<ClassVarListContext>
    {
        /// <summary>
        /// The variables names
        /// </summary>
        public ClassvarContextValues[] Variables { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; private set; }

        static internal ClassVarListContextValues Build(ClassVarListContext context)
        {
            if (context == null)
                return null;

            return new ClassVarListContextValues
            {
                Context = context,
                Variables = context.classvar().Select(q  => ClassvarContextValues.Build(q)).ToArray(),
                Type = context.DataType?.GetText(),
            };
        }
    }
}
