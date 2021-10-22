using System.Linq;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a Class_Context instance
    /// </summary>
    public class ClassContextValues : ContextValues<Class_Context>
    {
        // TODO: Extend with Implents, Modifiers, Attributes

        /// <summary>
        /// The class name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The base class for the class
        /// </summary>
        public string Inherits { get; internal set; }

        /// <summary>
        /// The interfaces, that the class implents
        /// </summary>
        public string[] Implents { get; internal set; }


        static internal ClassContextValues Build(Class_Context context)
        {
            if (context == null)
                return null;

            return new ClassContextValues
            {
                Context = context,
                Name = context.identifier()?.GetText(),
                Inherits = context.BaseType?.GetText(),
                Implents = (context._Implements?.Select(q => q.GetText()) ?? Enumerable.Empty<string>()).ToArray(),
            };
        }
    }
}
