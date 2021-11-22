using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a ConstructorchainContext instance
    /// </summary>
    public class ConstructorchainContextValues : ContextValues<ConstructorchainContext>
    {
        /// <summary>
        /// The arguments of the method call
        /// </summary>
        public NamedArgumentContextValues[] Arguments { get; internal set; }

        static internal ConstructorchainContextValues Build(ConstructorchainContext context)
        {
            if (context == null)
                return null;

            var arguments = (context.ArgList?._Args?.Select(q => NamedArgumentContextValues.Build(q)) ?? Enumerable.Empty<NamedArgumentContextValues>()).ToArray();
            return new ConstructorchainContextValues
            {
                Context = context,
                Arguments = arguments,
            };
        }
    }
}
