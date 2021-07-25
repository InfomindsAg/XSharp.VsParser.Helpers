using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Values
{
    public class ClassContextValues
    {
        // TODO: Extend with Implents, Modifiers, Attributes

        public string Name { get; internal set; }
        public string Inherits { get; internal set; }

        static internal ClassContextValues Build(Class_Context context)
        {
            if (context == null)
                return null;

            return new ClassContextValues
            {
                Name = context.identifier()?.GetText(),
                Inherits = context.BaseType?.GetText()
            };
        }
    }
}
