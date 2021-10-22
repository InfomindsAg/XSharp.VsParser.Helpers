using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a MethodContext instance
    /// </summary>
    public class PropertyContextValues : ContextValues<PropertyContext>
    {
        /// <summary>
        /// The type
        /// </summary>
        public string[] Modifiers { get; private set; }

        /// <summary>
        /// The method name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The return tyoe
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The CallingConvention
        /// </summary>
        public string CallingConvention { get; private set; }

        static internal PropertyContextValues Build(PropertyContext context)
        {
            if (context == null)
                return null;

            return new PropertyContextValues
            {
                Context = context,
                Modifiers = (context.Modifiers?._Tokens?.Select(q => q.Text) ?? Enumerable.Empty<string>()).ToArray(),
                Name = context.Id.GetText(),
                Type = context.Type?.GetText()
            };
        }
    }
}
