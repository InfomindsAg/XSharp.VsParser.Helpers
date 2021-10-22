using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a AccessMemberContext instance
    /// </summary>
    public class AccessMemberContextValues : ContextValues<AccessMemberContext>
    {
        /// <summary>
        /// The expression to access the member
        /// </summary>
        public string AccessExpression { get; internal set; }

        /// <summary>
        /// The member name
        /// </summary>
        public string MemberName { get; internal set; }

        /// <summary>
        /// True, if the member is accessed on super
        /// </summary>
        public bool IsSuperAccess { get; set; }

        static internal AccessMemberContextValues Build(AccessMemberContext context)
        {
            if (context == null)
                return null;

            return new AccessMemberContextValues
            {
                Context = context,
                AccessExpression = context.Expr?.GetText(),
                MemberName = context.Name?.GetText(),
                IsSuperAccess = context.Expr is PrimaryExpressionContext primaryExpression && primaryExpression.Expr is SuperExpressionContext,
            };
        }
    }
}