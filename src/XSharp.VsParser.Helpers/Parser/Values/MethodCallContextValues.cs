using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a MethodCallContext instance
    /// </summary>
    public class MethodCallContextValues : ContextValues<MethodCallContext>
    {
        /// <summary>
        /// The expression used to access the method
        /// </summary>
        public string MethodAccessExpression { get; internal set; }

        /// <summary>
        /// The method name
        /// </summary>
        public string MethodName { get; internal set; }

        /// <summary>
        /// The arguments of the method call
        /// </summary>
        public NamedArgumentContextValues[] Arguments { get; internal set; }

        static internal MethodCallContextValues Build(MethodCallContext context)
        {
            if (context == null)
                return null;

            var accessMember = context.Expr?.AsEnumerable().FirstOrDefaultType<AccessMemberContext>();
            var arguments = (context.ArgList?._Args?.Select(q => NamedArgumentContextValues.Build(q)) ?? Enumerable.Empty<NamedArgumentContextValues>()).ToArray();
            // var signature = context.;
            return new MethodCallContextValues
            {
                Context = context,
                MethodAccessExpression = accessMember?.Expr?.GetText(),
                MethodName = accessMember?.Name?.GetText(),
                Arguments = arguments,
            };
        }
    }
}
