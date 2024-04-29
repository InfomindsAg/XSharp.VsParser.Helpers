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
        /// The name expression, when the call is not an access member
        /// </summary>
        public NameExpressionContextValues NameExpression { get; internal set; }

        /// <summary>
        /// The access member values
        /// </summary>
        public AccessMemberContextValues AccessMember { get; internal set; }

        /// <summary>
        /// The arguments of the method call
        /// </summary>
        public NamedArgumentContextValues[] Arguments { get; internal set; }

        static internal MethodCallContextValues Build(MethodCallContext context)
        {
            if (context == null)
                return null;

            var accessMember = context.Expr?.AsEnumerable().FirstOrDefaultType<AccessMemberContext>();
            var nameExpression = accessMember == null ? context.Expr?.AsEnumerable().FirstOrDefaultType<NameExpressionContext>() : null;
            var arguments = (
                IsArgListEmpty(context.ArgList) ? Enumerable.Empty<NamedArgumentContextValues>() : context.ArgList._Args.Select(q => NamedArgumentContextValues.Build(q))
                ).ToArray();

            // var signature = context.;
            return new MethodCallContextValues
            {
                Context = context,
                AccessMember = AccessMemberContextValues.Build(accessMember),
                NameExpression = NameExpressionContextValues.Build(nameExpression),
                Arguments = arguments,
            };
        }

        static internal bool IsArgListEmpty(ArgumentListContext context)
        {
            return context?._Args == null || context._Args.Count == 0 || context._Args.All(q => NamedArgumentContextValues.IsEmpty(q));
        }
    }
}
